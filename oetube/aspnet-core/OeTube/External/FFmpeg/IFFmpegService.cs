namespace OeTube.External.FFmpeg
{
    public interface IFFmpegService
    {
        Task<ProbeInfo> AnalyzeAsync(string path, CancellationToken token = default);
        Task<ProbeInfo> AnalyzeAsync(string path, CancellationToken token = default);
        Task<FFmpegResult> ProcessAsync(FFmpegArgs args, Action<int>? progress = null, CancellationToken token = default);
    }
}