using OeTube.Domain.Configs;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths.VideoFiles;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Repositories;
using OeTube.Domain.Validators;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FileHandlers
{
    public class StartVideoUploadHandler : VideoUploadHandler<StartVideoUploadHandlerArgs>, IStartVideoUploadHandler, ITransientDependency
    {
        private readonly IUploadTaskFactory _uploadTaskFactory;

        public StartVideoUploadHandler(IFileContainerFactory fileContainerFactory,
                                  IBackgroundJobManager backgroundJobManager,
                                  IProcessUploadTaskFactory processUploadTaskFactory,
                                  IFFProbeService ffprobeService,
                                  IVideoFileValidator videoFileValidator,
                                  IVideoRepository repository,
                                  IVideoFileConfig config,
                                  IUploadTaskFactory uploadTaskFactory) : base(fileContainerFactory, backgroundJobManager, processUploadTaskFactory, ffprobeService, videoFileValidator, repository, config)
        {
            this._uploadTaskFactory = uploadTaskFactory;
        }

        public override async Task<Video> HandleFileAsync<TRelatedType>(StartVideoUploadHandlerArgs args, CancellationToken cancellationToken = default)
        {
            var container = _fileContainerFactory.Create<TRelatedType>();
            var sourceInfo = await _ffprobeService.AnalyzeAsync(args.Content, cancellationToken);
            _videoFileValidator.ValidateSourceVideo(sourceInfo);
            var sourceVideoStream = sourceInfo.VideoStreams[0];
            var resolution = sourceVideoStream.Resolution;

            var video = new Video(args.Id,
                    args.Name, args.CreatorId,
                    sourceInfo.Duration,
                    _config.GetDesiredResolutions(resolution))
                   .SetDescription(args.Description)
                   .SetAccess(args.Access);

            await container.SaveFileAsync(new SourcePath(video.Id), args.Content!, cancellationToken);
            if (_videoFileValidator.IsInDesiredResolutionAndFormat(sourceInfo))
            {
                await container.SaveFileAsync(new ResizedPath(video.Id, resolution), args.Content!, cancellationToken);
                video.Resolutions.Get(resolution).MarkReady();
            }
            if (args.IsWebAssemblyAvailable)
            {
                await ProcessUploadIfIsItReadyAsync(video, sourceInfo);
            }
            else if(!video.IsAllResolutionReady())
            {
             
                var resizingArgs=new VideoResizingArgs()
                {
                    Id = video.Id,
                    Tasks = video.GetResolutionsBy(false).Select(_uploadTaskFactory.Create).ToList()
                };
                foreach (var res in video.Resolutions)
                {
                    if (!res.IsReady)
                    {
                        res.MarkReady();
                    }
                }
                await _backgroundJobManager.EnqueueAsync(resizingArgs);
            }
            return video;
        }
    }

}