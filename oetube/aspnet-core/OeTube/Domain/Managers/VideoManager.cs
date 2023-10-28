using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Infrastructure.VideoStorage;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Events;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;

namespace OeTube.Domain.Managers
{
    public class VideoManager : DomainService, IQueryVideoRepository, IReadRepository<Video, Guid>, IUpdateVideoRepository, IDeleteRepository<Video, Guid>
    {
        private readonly IVideoRepository _repository;
        private readonly IVideoStorage _videoStorage;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IFFProbeService _ffprobeService;
        private readonly IFFmpegService _ffmpegService;
        private readonly IVideoFileValidator _videoFileValidator;
        private readonly IProcessUploadTaskFactory _processUploadTaskFactory;
        private readonly ILocalEventBus _localEventBus;

        public VideoFileConfig Config { get; }
        public IVideoStorageRead Files => _videoStorage;

        public VideoManager(IVideoRepository repository,
                            IVideoStorage videoStorage,
                            IBackgroundJobManager backgroundJobManager,
                            IFFProbeService ffprobeService,
                            IFFmpegService ffmpegService,
                            IVideoFileValidator fileValidator,
                            IProcessUploadTaskFactory processUploadTaskFactory,
                            IVideoFileConfigFactory videoFileConfigFactory,
                            ILocalEventBus localEventBus)
        {
            _repository = repository;
            _videoStorage = videoStorage;
            _backgroundJobManager = backgroundJobManager;
            _ffprobeService = ffprobeService;
            _ffmpegService = ffmpegService;
            _videoFileValidator = fileValidator;
            _processUploadTaskFactory = processUploadTaskFactory;
            Config = videoFileConfigFactory.Create();
            _localEventBus = localEventBus;
        }

        public async Task<Video> StartUploadAsync(string name, string? description, AccessType access, Guid? creatorId, ByteContent content)
        {
            var sourceInfo = await _ffprobeService.AnalyzeAsync(content);
            _videoFileValidator.ValidateSourceVideo(sourceInfo);

            var sourceVideoStream = sourceInfo.VideoStreams[0];
            var resolution = sourceVideoStream.Resolution;

            var video = new Video(GuidGenerator.Create(),
                         name, creatorId,
                         sourceInfo.Duration,
                         Config.GetDesiredResolutions(resolution))
                        .SetDescription(description)
                        .SetAccess(access);

            await _videoStorage.SaveSourceAsync(video.Id, content);
            if (_videoFileValidator.IsInDesiredResolutionAndFormat(sourceInfo))
            {
                await _videoStorage.SaveResizedAsync(video.Id, resolution, content);
                video.Resolutions.Get(resolution).MarkReady();
            }
            await _repository.InsertAsync(video, true);
            await ProcessUploadIfIsItReadyAsync(video, sourceInfo);
            return video;
        }

        public async Task<Video> ContinueUploadAsync(Video video, ByteContent content)
        {
            var sourceInfo = await _ffprobeService.AnalyzeAsync(await _videoStorage.ReadSourceAsync(video.Id));
            var resizedInfo = await _ffprobeService.AnalyzeAsync(content);

            _videoFileValidator.ValidateResizedVideo(video, sourceInfo, resizedInfo);
            var resolution = resizedInfo.VideoStreams[0].Resolution;

            await _videoStorage.SaveResizedAsync(video.Id, resolution, content);

            video.Resolutions.Get(resolution).MarkReady();

            await _repository.UpdateAsync(video, true);
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
                await _backgroundJobManager.EnqueueAsync(processUploadTask);
            }
        }

        public async Task ProcessUploadAsync(ProcessUploadTask uploadTask, CancellationToken cancellationToken = default)
        {
            var video = await _repository.GetAsync(uploadTask.Id);
            if (video.IsAllResolutionReady())
            {
                await ExtractFramesAsync(uploadTask, cancellationToken);
                await CreateHlsStreamAsync(uploadTask, cancellationToken);

                if (uploadTask.IsUploadReady)
                {
                    video.SetUploadCompleted();
                    await _repository.UpdateAsync(video, true, cancellationToken);
                    await _videoStorage.DeleteSourceAsync(uploadTask.Id, cancellationToken);
                    await _localEventBus.PublishAsync(new VideoUploadCompletedEto(video));
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
                var target = await _videoStorage.ReadResizedAsync(id, targetResolution, cancellationToken);
                var result = await _ffmpegService.ExecuteAsync(target, arguments, arguments, cancellationToken);
                foreach (var item in result.OutputFiles)
                {
                    var content = await _ffmpegService.GetContentAsync(item, cancellationToken);
                    int index = int.Parse(content.FileNameWithoutFormat);
                    await _videoStorage.SaveFrameAsync(id, index, content, cancellationToken);
                    if (cancellationToken.IsCancellationRequested) break;
                }
                await _videoStorage.SaveSelectedFrameAsync(id, 1, cancellationToken);
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
                    var content = await _videoStorage.ReadResizedAsync(id, resolution, cancellationToken);
                    var result = await _ffmpegService.ExecuteAsync(content, arguments, arguments, cancellationToken);
                    foreach (var item in result.OutputFiles)
                    {
                        var output = await _ffmpegService.GetContentAsync(item, cancellationToken);
                        if (output.FileName == hlsListName)
                        {
                            await _videoStorage.SaveHlsListAsync(id, resolution, output, cancellationToken);
                        }
                        else if (output.Format == hlstSegmentFormat)
                        {
                            int segment = int.Parse(output.FileNameWithoutFormat);
                            await _videoStorage.SaveHlsSegmentAsync
                                (id, resolution, segment, output, cancellationToken);
                        }
                        if (cancellationToken.IsCancellationRequested) break;
                    }

                    if (cancellationToken.IsCancellationRequested) break;
                    await _videoStorage.DeleteResizedAsync(id, resolution, cancellationToken);
                }
                finally
                {
                    await _ffmpegService.CleanUpAsync();
                }
            }
        }

        public async Task SelectFrameAsync(Guid id, int index, CancellationToken cancellationToken = default)
        {
            await _videoStorage.SaveSelectedFrameAsync(id, index, cancellationToken);
        }

        public async Task<Video> UpdateAsync(Video entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return await _repository.UpdateAsync(entity, autoSave, cancellationToken);
        }

        public async Task<Video> UpdateAccessGroupsAsync(Video entity, IEnumerable<Guid> groupIds, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return await _repository.UpdateAccessGroupsAsync(entity, groupIds, autoSave, cancellationToken);
        }

        public async Task UpdateManyAsync(IEnumerable<Video> entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await _repository.UpdateManyAsync(entity, autoSave, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await _repository.DeleteAsync(id, autoSave, cancellationToken);
            if (autoSave)
            {
                await _videoStorage.DeleteAllAsync(id, cancellationToken);
            }
        }

        public async Task DeleteAsync(Video entity, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(entity.Id, autoSave, cancellationToken);
        }

        public async Task DeleteUncompletedVideosAsync(TimeSpan old, CancellationToken cancellationToken = default)
        {
            var videos = await _repository.GetUncompletedVideosAsync(old, null, false, cancellationToken);
            await DeleteManyAsync(videos, true, cancellationToken);
        }

        public async Task DeleteManyAsync(IEnumerable<Guid> ids, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await _repository.DeleteManyAsync(ids, autoSave, cancellationToken);
            if (autoSave)
            {
                await Task.WhenAll(ids.Select(id => _videoStorage.DeleteAllAsync(id, cancellationToken)));
            }
        }

        public async Task DeleteManyAsync(IEnumerable<Video> entities, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            await _repository.DeleteManyAsync(entities, autoSave, cancellationToken);
            if (autoSave)
            {
                await Task.WhenAll(entities.Select(e => _videoStorage.DeleteAllAsync(e.Id, cancellationToken)));
            }
        }

        public Task<Video> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return _repository.GetAsync(id, includeDetails, cancellationToken);
        }

        public Task<List<Video>> GetManyAsync(IEnumerable<Guid> ids, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return _repository.GetManyAsync(ids, includeDetails, cancellationToken);
        }

        public Task<List<Video>> GetListAsync(IVideoQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return _repository.GetListAsync(args, includeDetails, cancellationToken);
        }

        public Task<List<Group>> GetAccessGroupsAsync(Video video, IGroupQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return _repository.GetAccessGroupsAsync(video, args, includeDetails, cancellationToken);
        }

        public Task<List<Video>> GetUncompletedVideosAsync(TimeSpan? old = null, IVideoQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return _repository.GetUncompletedVideosAsync(old, args, includeDetails, cancellationToken);
        }
    }
}