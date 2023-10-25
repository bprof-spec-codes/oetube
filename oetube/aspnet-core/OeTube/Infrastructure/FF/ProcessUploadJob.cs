using OeTube.Domain.Repositories;
using OeTube.Events;
using OeTube.Infrastructure.FF.Mpeg;
using OeTube.Infrastructure.VideoStorages;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;

namespace OeTube.Infrastructure.FF
{
    public class ProcessUploadJob : AsyncBackgroundJob<ProcessUploadTask>, ITransientDependency
    {
        private const int FrameNumber = 10;
        private const string FrameName = "%d.jpeg";
        private const string HlsListName = "list.m3u8";
        private const string HlsSegmentFormat = "ts";
        private const string HlsSegmentName = "%d." + HlsSegmentFormat;

        private readonly VideoStorage _videoStorage;
        private readonly IVideoRepository _videoRepository;
        private readonly IFFmpegService _ffmpeg;
        private readonly ILocalEventBus _localEventBus;

        public ProcessUploadJob(VideoStorage videoStorage, IVideoRepository videoRepository, IFFmpegService ffmpeg, ILocalEventBus localEventBus)
        {
            _videoStorage = videoStorage;
            _videoRepository = videoRepository;
            _ffmpeg = ffmpeg;
            _localEventBus = localEventBus;
        }

        private async Task ExtractFramesAsync(ProcessUploadTask processUploadTask, CancellationToken cancellationToken = default)
        {
            Guid id = processUploadTask.Id;
            var targetResolution = processUploadTask.ExtractFrameTarget;
            string arguments = processUploadTask.GetExractFramesArguments(FrameName, FrameNumber);
            try
            {
                var target = await _videoStorage.Get.ResizedAsync(id, targetResolution, cancellationToken);
                var result = await _ffmpeg.ExecuteAsync(target, arguments, arguments, cancellationToken);
                foreach (var item in result.OutputFiles)
                {
                    var content = await _ffmpeg.GetContentAsync(item, cancellationToken);
                    int index = int.Parse(content.FileNameWithoutFormat);
                    await _videoStorage.Save.FrameAsync(id, index, content, cancellationToken);
                    if (cancellationToken.IsCancellationRequested) break;
                }
                await _videoStorage.Save.SelectedFrameAsync(id, 1, cancellationToken);
            }
            finally
            {
                await _ffmpeg.CleanUpAsync();
            }

        }
        private async Task CreateHlsStreamAsync(ProcessUploadTask processUploadTask, CancellationToken cancellationToken = default)
        {
            Guid id = processUploadTask.Id;
            string arguments = processUploadTask.GetHlsArguments(HlsListName, HlsSegmentName);
            foreach (var resolution in processUploadTask.Resolutions)
            {
                try
                {
                    var content = await _videoStorage.Get.ResizedAsync(id, resolution, cancellationToken);
                    var result = await _ffmpeg.ExecuteAsync(content, arguments, arguments, cancellationToken);
                    foreach (var item in result.OutputFiles)
                    {
                        var output = await _ffmpeg.GetContentAsync(item, cancellationToken);
                        if (output.FileName == HlsListName)
                        {
                            await _videoStorage.Save.HlsListAsync(id, resolution, output, cancellationToken);
                        }
                        else if (output.Format == HlsSegmentFormat)
                        {
                            int segment = int.Parse(output.FileNameWithoutFormat);
                            await _videoStorage.Save.HlsSegmentAsync
                                (id, resolution, segment, output, cancellationToken);
                        }
                        if (cancellationToken.IsCancellationRequested) break;
                    }

                    if (cancellationToken.IsCancellationRequested) break;
                    await _videoStorage.Delete.ResizedAsync(id, resolution, cancellationToken);
                }
                finally
                {
                    await _ffmpeg.CleanUpAsync();
                }
            }
        }

        public async override Task ExecuteAsync(ProcessUploadTask args)
        {
            await ExtractFramesAsync(args);
            await CreateHlsStreamAsync(args);

            if (args.IsUploadReady)
            {
                var video = await _videoRepository.GetAsync(args.Id, true);
                video.SetUploadCompleted();
                await _videoRepository.UpdateAsync(video, true);
                await _videoStorage.Delete.SourceAsync(args.Id);
                await _localEventBus.PublishAsync(new VideoUploadCompletedEto(video));
            }
        }
    }
}
