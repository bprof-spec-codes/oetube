using OeTube.Domain.Configs;
using OeTube.Domain.FilePaths.VideoFiles;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Repositories;
using OeTube.Infrastructure.FFMpeg;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;

namespace OeTube.Infrastructure.FileHandlers
{
    public class VideoResizingHandler:IVideoResizingHandler,ITransientDependency
    {
        private readonly IFileContainerFactory _fileContainerFactory;
        private readonly IVideoRepository _repository;
        private readonly IFFMpegService _ffmpegService;
        private readonly ILocalEventBus _localEventBus;
        private readonly IVideoFileConfig _config;
        private readonly IProcessUploadTaskFactory _processUploadTaskFactory;
        private readonly IFFProbeService _ffProbeService;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public VideoResizingHandler(IFileContainerFactory fileContainerFactory,
                                    IVideoRepository repository,
                                    IFFMpegService ffmpegService,
                                    ILocalEventBus localEventBus,
                                    IVideoFileConfig config,
                                    IProcessUploadTaskFactory processUploadTaskFactory,
                                    IFFProbeService ffProbeService,
                                    IBackgroundJobManager backgroundJobManager)
        {
            _fileContainerFactory = fileContainerFactory;
            _repository = repository;
            _ffmpegService = ffmpegService;
            _localEventBus = localEventBus;
            _config = config;
            _processUploadTaskFactory = processUploadTaskFactory;
            _ffProbeService = ffProbeService;
            _backgroundJobManager = backgroundJobManager;
        }

        public async Task HandleFileAsync<TRelatedType>(VideoResizingArgs args, CancellationToken cancellationToken = default)
        {
            try
            {
                var container = _fileContainerFactory.Create<TRelatedType>();
                var source = await _repository.GetAsync(args.Id);
                var sourceContent = await container.GetFileAsync(new SourcePath(args.Id), cancellationToken);
                string singleArgs =string.Join(" ", args.Tasks.Select((t, i) => t.Arguments + $" {i}.{_config.OutputFormat}"));

                var result = await _ffmpegService.ExecuteAsync(sourceContent, singleArgs, singleArgs, cancellationToken);

                foreach (var item in result.OutputFiles)
                {
                    var resized = await _ffmpegService.GetContentAsync(item, cancellationToken);
                    var index = int.Parse(Path.GetFileNameWithoutExtension(item));
                    var resolution = args.Tasks[index].Resolution;
                    await container.SaveFileAsync(new ResizedPath(args.Id, resolution), resized, cancellationToken);
                }
                var sourceInfo = await _ffProbeService.AnalyzeAsync(sourceContent);
                var processUploadTask = _processUploadTaskFactory.Create(source, sourceInfo);
                await _backgroundJobManager.EnqueueAsync(processUploadTask);
            }
            finally
            {
                await _ffmpegService.CleanUpAsync();
            }
        }
    }
}