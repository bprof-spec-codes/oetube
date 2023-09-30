using OeTube.Infrastructure.FFmpeg.Info;
using OeTube.Infrastructure.ProcessTemplate;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFmpeg
{

    public interface IFFmpegService
    {
        public string WorkingDirectory
        {
            get; set;
        }

        public bool WriteToDebug
        {
            get; set;
        }

        Task<ProbeInfo> AnalyzeAsync(string path, CancellationToken cancellationToken = default);
        Task<ProcessResult> ConvertAsync(NamedArguments args, CancellationToken cancellationToken = default);
        Task BulkBackgroundConvertAsync(Guid jobId, NamedArguments[] args);
    }

}