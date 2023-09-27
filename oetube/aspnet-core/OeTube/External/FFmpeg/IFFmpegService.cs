using Volo.Abp.DependencyInjection;

namespace OeTube.External.FFmpeg
{
    public interface IFFmpegService:ITransientDependency
    {
        Task<ProbeInfo> AnalyzeAsync(string path, CancellationToken cancellationToken = default);
        Task<FFmpegResult> ProcessAsync(FFmpegArgs args, Action<int>? progress = null, CancellationToken cancellationToken = default);
    }
}