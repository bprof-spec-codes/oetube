using OeTube.Infrastructure.FFmpeg.Info;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFmpeg
{
    public interface IFFmpegService
    {
        Task<ProbeInfo> AnalyzeAsync(string path, CancellationToken cancellationToken = default);
        Task<FFmpegResult> ProcessAsync(FFmpegCommand command, Action<FFmpegCommand,int>? progress = null, CancellationToken cancellationToken = default);
    }
}