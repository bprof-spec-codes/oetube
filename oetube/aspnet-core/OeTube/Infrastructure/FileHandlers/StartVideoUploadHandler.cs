using OeTube.Configs;
using OeTube.Domain.Configs;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths.VideoFiles;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Validators;
using OeTube.Infrastructure.FileContainers;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FileHandlers
{
    public class StartVideoUploadHandler : VideoUploadHandler<StartVideoUploadHandlerArgs>,IStartVideoUploadHandler,ITransientDependency
    {
        public StartVideoUploadHandler(IFileContainerFactory fileContainerFactory,
                                IBackgroundJobManager backgroundJobManager,
                                  IProcessUploadTaskFactory processUploadTaskFactory,
                                  IFFProbeService ffprobeService,
                                  IVideoFileValidator videoFileValidator,
                                  IVideoRepository repository,
                                  IVideoFileConfig config) : base(fileContainerFactory,backgroundJobManager, processUploadTaskFactory, ffprobeService, videoFileValidator, repository, config)
        {
        }

        public override async Task<Video> HandleFileAsync<TRelatedType>(ByteContent content, StartVideoUploadHandlerArgs args, CancellationToken cancellationToken = default)
        {
            var container = _fileContainerFactory.Create<TRelatedType>();
            var sourceInfo = await _ffprobeService.AnalyzeAsync(content, cancellationToken);
            _videoFileValidator.ValidateSourceVideo(sourceInfo);
            var sourceVideoStream = sourceInfo.VideoStreams[0];
            var resolution = sourceVideoStream.Resolution;

            var video = new Video(args.Id,
                    args.Name, args.CreatorId,
                    sourceInfo.Duration,
                    _config.GetDesiredResolutions(resolution))
                   .SetDescription(args.Description)
                   .SetAccess(args.Access);

            await container.SaveFileAsync(new SourcePath(video.Id), content, cancellationToken);
            if (_videoFileValidator.IsInDesiredResolutionAndFormat(sourceInfo))
            {
                await container.SaveFileAsync(new ResizedPath(video.Id, resolution), content, cancellationToken);
                video.Resolutions.Get(resolution).MarkReady();
            }
            await _repository.InsertAsync(video, true, cancellationToken);
            await ProcessUploadIfIsItReadyAsync(video, sourceInfo);
            return video;
        }
    }
}