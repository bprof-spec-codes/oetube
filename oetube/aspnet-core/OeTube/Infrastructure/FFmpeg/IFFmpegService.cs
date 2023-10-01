using OeTube.Infrastructure.FFmpeg.Info;
using OeTube.Infrastructure.ProcessTemplate;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFmpeg
{

    public interface IFFmpegService
    {
        string WorkingDirectory
        {
            get; set;
        }

        bool WriteToDebug
        {
            get; set;
        }

        Task<ProbeInfo> AnalyzeAsync(string path, CancellationToken cancellationToken = default);
        Task<ProcessResult> ConvertAsync(NamedArguments args, CancellationToken cancellationToken = default);
        Task BulkBackgroundConvertAsync(Guid jobId, NamedArguments[] args);
    }

}