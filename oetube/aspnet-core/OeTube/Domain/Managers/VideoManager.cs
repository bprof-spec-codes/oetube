using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;
using OeTube.Domain.Infrastructure.FileClasses;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Events;
using OeTube.Infrastructure.FileClasses.VideoFiles;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.EventBus.Local;

namespace OeTube.Domain.Managers
{
    public class VideoManager : DomainManager<IVideoRepository,Video,Guid,IVideoQueryArgs,IVideoFileClass>, IQueryVideoRepository, IReadRepository<Video, Guid>, IUpdateVideoRepository,IDeleteRepository<Video,Guid>
    {
        private readonly IFFProbeService _ffprobeService;
        private readonly IFFmpegService _ffmpegService;
        private readonly IVideoFileValidator _videoFileValidator;
        private readonly IProcessUploadTaskFactory _processUploadTaskFactory;
        public VideoFileConfig Config { get; }

        public VideoManager(IVideoRepository repository,
                            IFileContainerFactory containerFactory,
                            IBackgroundJobManager backgroundJobManager,
                            IFFProbeService ffprobeService,
                            IFFmpegService ffmpegService,
                            IVideoFileValidator fileValidator,
                            IProcessUploadTaskFactory processUploadTaskFactory,
                            IVideoFileConfigFactory videoFileConfigFactory,
                            ILocalEventBus localEventBus):base(repository,containerFactory,backgroundJobManager,localEventBus)
        {
            _ffprobeService = ffprobeService;
            _ffmpegService = ffmpegService;
            _videoFileValidator = fileValidator;
            _processUploadTaskFactory = processUploadTaskFactory;
            Config = videoFileConfigFactory.Create();
        }

        public async Task<Video> StartUploadAsync(string name, string? description, AccessType access, Guid? creatorId, ByteContent content,CancellationToken cancellationToken=default)
        {
            var sourceInfo = await _ffprobeService.AnalyzeAsync(content,cancellationToken);
            _videoFileValidator.ValidateSourceVideo(sourceInfo);

            var sourceVideoStream = sourceInfo.VideoStreams[0];
            var resolution = sourceVideoStream.Resolution;

            var video = new Video(GuidGenerator.Create(),
                         name, creatorId,
                         sourceInfo.Duration,
                         Config.GetDesiredResolutions(resolution))
                        .SetDescription(description)
                        .SetAccess(access);

            await FileContainer.SaveFileAsync(new SourceFileClass(video.Id), content,cancellationToken);
            if (_videoFileValidator.IsInDesiredResolutionAndFormat(sourceInfo))
            {
                await FileContainer.SaveFileAsync(new ResizedFileClass(video.Id,resolution), content,cancellationToken);
                video.Resolutions.Get(resolution).MarkReady();
            }
            await Repository.InsertAsync(video, true);
            await ProcessUploadIfIsItReadyAsync(video, sourceInfo);
            return video;
        }

        public async Task<Video> ContinueUploadAsync(Video video, ByteContent content,CancellationToken cancellationToken=default)
        {
            var sourceFile = await FileContainer.GetFileAsync(new SourceFileClass(video.Id), cancellationToken);
            var sourceInfo = await _ffprobeService.AnalyzeAsync(sourceFile,cancellationToken);
            var resizedInfo = await _ffprobeService.AnalyzeAsync(content,cancellationToken);

            _videoFileValidator.ValidateResizedVideo(video, sourceInfo, resizedInfo);
            var resolution = resizedInfo.VideoStreams[0].Resolution;

            await FileContainer.SaveFileAsync(new ResizedFileClass(video.Id,resolution), content);

            video.Resolutions.Get(resolution).MarkReady();

            await Repository.UpdateAsync(video, true,cancellationToken);
            await ProcessUploadIfIsItReadyAsync(video, sourceInfo);
            return video;
        }

        private async Task ProcessUploadIfIsItReadyAsync(Video video, VideoInfo videoInfo)
        {
            if (video.IsAllResolutionReady())
            {
                var resolutions = video.GetResolutionsBy(true).ToArray();
                var extractFrameTarget = resolutions.OrderByDescending(r => r.Height).First();
                var processUploadTask = _processUploadTaskFactory.Create(video, videoInfo);
                await BackgroundJobManager.EnqueueAsync(processUploadTask);
            }
        }

        public async Task ProcessUploadAsync(ProcessUploadTask uploadTask, CancellationToken cancellationToken = default)
        {
            var video = await Repository.GetAsync(uploadTask.Id);
            if (video.IsAllResolutionReady())
            {
                await ExtractFramesAsync(uploadTask, cancellationToken);
                await CreateHlsStreamAsync(uploadTask, cancellationToken);

                if (uploadTask.IsUploadReady)
                {
                    video.SetUploadCompleted();
                    await Repository.UpdateAsync(video, true, cancellationToken);
                    await FileContainer.DeleteFileAsync(new SourceFileClass(video.Id), cancellationToken);
                    await LocalEventBus.PublishAsync(new VideoUploadCompletedEto(video));
                }
            }
        }

        private async Task ExtractFramesAsync(ProcessUploadTask processUploadTask, CancellationToken cancellationToken = default)
        {
            Guid id = processUploadTask.Id;
            var targetResolution = processUploadTask.ExtractFrameTarget;
            string frameName = "%d.jpeg";
            string arguments = processUploadTask.GetExractFramesArguments(frameName);
            try
            {
                var target = await FileContainer.GetFileAsync(new ResizedFileClass(id,targetResolution), cancellationToken);
                var result = await _ffmpegService.ExecuteAsync(target, arguments, arguments, cancellationToken);
                foreach (var item in result.OutputFiles)
                {
                    var content = await _ffmpegService.GetContentAsync(item, cancellationToken);
                    int index = int.Parse(Path.GetFileNameWithoutExtension(item));
                    await FileContainer.SaveFileAsync(new FrameFileClass(id,index), content, cancellationToken);
                    if (cancellationToken.IsCancellationRequested) break;
                }
                await SelectFrameAsync(id, 1, cancellationToken);
            }
            finally
            {
                await _ffmpegService.CleanUpAsync();
            }
        }

        private async Task CreateHlsStreamAsync(ProcessUploadTask processUploadTask, CancellationToken cancellationToken = default)
        {
            Guid id = processUploadTask.Id;
            string hlsListName = "list.m3u8";
            string hlstSegmentFormat = "ts";
            string hlsSegmentName = "%d." + hlstSegmentFormat;

            string arguments = processUploadTask.GetHlsArguments(hlsListName, hlsSegmentName);
            foreach (var resolution in processUploadTask.Resolutions)
            {
                try
                {
                    var content = await FileContainer.GetFileAsync(new ResizedFileClass(id, resolution), cancellationToken);
                    var result = await _ffmpegService.ExecuteAsync(content, arguments, arguments, cancellationToken);
                    foreach (var fileName in result.OutputFiles)
                    {
                        var output = await _ffmpegService.GetContentAsync(fileName, cancellationToken);
                        if (fileName == hlsListName)
                        {
                            await FileContainer.SaveFileAsync(new HlsListFileClass(id, resolution), output, cancellationToken);
                        }
                        else if (output.Format == hlstSegmentFormat)
                        {
                            int segment = int.Parse(Path.GetFileNameWithoutExtension(fileName));
                            await FileContainer.SaveFileAsync(new HlsSegmentFileClass(id, resolution, segment), output, cancellationToken);
                        }
                        if (cancellationToken.IsCancellationRequested) break;
                    }

                    if (cancellationToken.IsCancellationRequested) break;
                    await FileContainer.DeleteFileAsync(new ResizedFileClass(id, resolution), cancellationToken);
                }
                finally
                {
                    await _ffmpegService.CleanUpAsync();
                }
            }
        }

        public async Task SelectFrameAsync(Guid id, int index, CancellationToken cancellationToken = default)
        {
            var selectedFrame = await FileContainer.GetFileAsync(new FrameFileClass(id, index), cancellationToken);
            await FileContainer.SaveFileAsync(new SelectedFrameFileClass(id), selectedFrame, cancellationToken);
        }

      
        public async Task<Video> UpdateAccessGroupsAsync(Video entity, IEnumerable<Guid> groupIds, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return await Repository.UpdateAccessGroupsAsync(entity, groupIds, autoSave, cancellationToken);
        }
       
        public Task<List<Group>> GetAccessGroupsAsync(Video video, IGroupQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetAccessGroupsAsync(video, args, includeDetails, cancellationToken);
        }

        public Task<List<Video>> GetUncompletedVideosAsync(TimeSpan? old = null, IQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetUncompletedVideosAsync(old, args, includeDetails, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Repository.DeleteAsync(id, autoSave, cancellationToken);
        }

        public async Task DeleteAsync(Video entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Repository.DeleteAsync(entity, autoSave, cancellationToken);
        }

        public async Task DeleteManyAsync(IEnumerable<Guid> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Repository.DeleteManyAsync(ids, autoSave, cancellationToken);
        }

        public async Task DeleteManyAsync(IEnumerable<Video> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Repository.DeleteManyAsync(entities, autoSave, cancellationToken);
        }
    }
}