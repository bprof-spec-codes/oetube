using OeTube.Domain.Configs;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Repositories;
using OeTube.Domain.Validators;
using Volo.Abp.BackgroundJobs;

namespace OeTube.Infrastructure.FileHandlers
{
    public abstract class VideoUploadHandler<TArgs> : IFileHandler<TArgs, Video>
    {
        protected readonly IFileContainerFactory _fileContainerFactory;
        protected readonly IBackgroundJobManager _backgroundJobManager;
        protected readonly IProcessUploadTaskFactory _processUploadTaskFactory;
        protected readonly IFFProbeService _ffprobeService;
        protected readonly IVideoFileValidator _videoFileValidator;
        protected readonly IVideoRepository _repository;
        protected readonly IVideoFileConfig _config;

        protected VideoUploadHandler(IFileContainerFactory fileContainerFactory,
                                     IBackgroundJobManager backgroundJobManager,
                                     IProcessUploadTaskFactory processUploadTaskFactory,
                                     IFFProbeService ffprobeService,
                                     IVideoFileValidator videoFileValidator,
                                     IVideoRepository repository,
                                     IVideoFileConfig config)
        {
            _fileContainerFactory = fileContainerFactory;
            _backgroundJobManager = backgroundJobManager;
            _processUploadTaskFactory = processUploadTaskFactory;
            _ffprobeService = ffprobeService;
            _videoFileValidator = videoFileValidator;
            _repository = repository;
            _config = config;
        }

        public abstract Task<Video> HandleFileAsync<TRelatedType>(TArgs args, CancellationToken cancellationToken = default);

        protected async Task ProcessUploadIfIsItReadyAsync(Video video, VideoInfo videoInfo)
        {
            if (video.IsAllResolutionReady())
            {
                var processUploadTask = _processUploadTaskFactory.Create(video, videoInfo);
                await _backgroundJobManager.EnqueueAsync(processUploadTask);
            }
        }
    }
}