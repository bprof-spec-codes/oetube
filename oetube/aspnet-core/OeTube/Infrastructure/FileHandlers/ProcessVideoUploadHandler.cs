using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths.VideoFiles;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Repositories;
using OeTube.Events;
using OeTube.Infrastructure.FFMpeg;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;

namespace OeTube.Infrastructure.FileHandlers
{
    public class ProcessVideoUploadHandler : IProcessVideoUploadHandler, ITransientDependency
    {
        private readonly IFileContainerFactory _fileContainerFactory;
        private readonly IVideoRepository _repository;
        private readonly IFFMpegService _ffmpegService;
        private readonly ILocalEventBus _localEventBus;
        private readonly ISelectVideoFrameHandler _selectFrameHandler;

        public ProcessVideoUploadHandler(IFileContainerFactory fileContainerFactory,
                                        IVideoRepository repository,
                                         IFFMpegService ffmpegService,
                                         ILocalEventBus localEventBus,
                                         ISelectVideoFrameHandler selectFrameHandler)
        {
            this._fileContainerFactory = fileContainerFactory;
            _repository = repository;
            _ffmpegService = ffmpegService;
            _localEventBus = localEventBus;
            this._selectFrameHandler = selectFrameHandler;
        }

        public async Task HandleFileAsync<TRelatedType>(ProcessVideoUploadArgs args, CancellationToken cancellationToken = default)
        {
            var container = _fileContainerFactory.Create<TRelatedType>();
            var video = await _repository.GetAsync(args.Id);
            if (video.IsAllResolutionReady())
            {
                await ExtractFramesAsync<TRelatedType>(container, args, cancellationToken);
                await CreateHlsStreamAsync(container, args, cancellationToken);

                if (args.IsUploadReady)
                {
                    video.SetUploadCompleted();
                    await _repository.UpdateAsync(video, true, cancellationToken);
                    await container.DeleteFileAsync(new SourcePath(video.Id), cancellationToken);
                    await _localEventBus.PublishAsync(new VideoUploadCompletedEto(video));
                }
            }
        }

        private async Task ExtractFramesAsync<TRelatedType>(IFileContainer container, ProcessVideoUploadArgs args, CancellationToken cancellationToken = default)
        {
            Guid id = args.Id;
            var targetResolution = args.ExtractFrameTarget;
            string frameName = "%d.jpeg";
            string arguments = args.GetExractFramesArguments(frameName);
            try
            {
                var target = await container.GetFileAsync(new ResizedPath(id, targetResolution), cancellationToken);
                var result = await _ffmpegService.ExecuteAsync(target, arguments, arguments, cancellationToken);
                foreach (var item in result.OutputFiles)
                {
                    var content = await _ffmpegService.GetContentAsync(item, cancellationToken);
                    int index = int.Parse(Path.GetFileNameWithoutExtension(item));
                    await container.SaveFileAsync(new FramePath(id, index), content, cancellationToken);
                    if (cancellationToken.IsCancellationRequested) break;
                }
                await _selectFrameHandler.HandleFileAsync<TRelatedType>(new SelectVideoFrameHandlerArgs(id, 1), cancellationToken);
            }
            finally
            {
                await _ffmpegService.CleanUpAsync();
            }
        }

        private async Task CreateHlsStreamAsync(IFileContainer container, ProcessVideoUploadArgs processUploadTask, CancellationToken cancellationToken = default)
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
                    var content = await container.GetFileAsync(new ResizedPath(id, resolution), cancellationToken);
                    var result = await _ffmpegService.ExecuteAsync(content, arguments, arguments, cancellationToken);
                    foreach (var fileName in result.OutputFiles)
                    {
                        var output = await _ffmpegService.GetContentAsync(fileName, cancellationToken);
                        if (fileName == hlsListName)
                        {
                            await container.SaveFileAsync(new HlsListPath(id, resolution), output, cancellationToken);
                        }
                        else if (output.Format == hlstSegmentFormat)
                        {
                            int segment = int.Parse(Path.GetFileNameWithoutExtension(fileName));
                            await container.SaveFileAsync(new HlsSegmentPath(id, resolution, segment), output, cancellationToken);
                        }
                        if (cancellationToken.IsCancellationRequested) break;
                    }

                    if (cancellationToken.IsCancellationRequested) break;
                    await container.DeleteFileAsync(new ResizedPath(id, resolution), cancellationToken);
                }
                finally
                {
                    await _ffmpegService.CleanUpAsync();
                }
            }
        }
    }
}