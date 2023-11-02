using OeTube.Configs;
using OeTube.Domain.Configs;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths.VideoFiles;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Repositories;
using OeTube.Domain.Validators;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FileHandlers
{
    public class ContinueVideoUploadHandler : VideoUploadHandler<ContinueVideoUploadHandlerArgs>, IContinueVideoUploadHandler,ITransientDependency
    {
        public ContinueVideoUploadHandler(IFileContainerFactory fileContainerFactory,
                                     IBackgroundJobManager backgroundJobManager,
                                     IProcessUploadTaskFactory processUploadTaskFactory,
                                     IFFProbeService ffprobeService,
                                     IVideoFileValidator videoFileValidator,
                                     IVideoRepository repository,
                                     IVideoFileConfig config) : base(fileContainerFactory,backgroundJobManager, processUploadTaskFactory, ffprobeService, videoFileValidator, repository, config)
        {
        }

        public override async Task<Video> HandleFileAsync<TRelatedType>(ByteContent content, ContinueVideoUploadHandlerArgs args, CancellationToken cancellationToken = default)
        {
            var container = _fileContainerFactory.Create<TRelatedType>();
            var sourceFile = await container.GetFileAsync(new SourcePath(args.Video.Id), cancellationToken);
            var sourceInfo = await _ffprobeService.AnalyzeAsync(sourceFile, cancellationToken);
            var resizedInfo = await _ffprobeService.AnalyzeAsync(content, cancellationToken);

            _videoFileValidator.ValidateResizedVideo(args.Video, sourceInfo, resizedInfo);
            var resolution = resizedInfo.VideoStreams[0].Resolution;

            await container.SaveFileAsync(new ResizedPath(args.Video.Id, resolution), content, cancellationToken);
            args.Video.Resolutions.Get(resolution).MarkReady();

            await _repository.UpdateAsync(args.Video, true, cancellationToken);
            await ProcessUploadIfIsItReadyAsync(args.Video, sourceInfo);
            return args.Video;
        }
    }
}