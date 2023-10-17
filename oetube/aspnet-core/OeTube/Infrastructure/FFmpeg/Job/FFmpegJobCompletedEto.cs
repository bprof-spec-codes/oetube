using OeTube.Infrastructure.ProcessTemplate;
using Volo.Abp.EventBus;

namespace OeTube.Infrastructure.FFmpeg.Job
{
    [EventName(nameof(FFmpegJobCompletedEto))]
    public record FFmpegJobCompletedEto(Guid JobId, IReadOnlyList<ProcessResult> Results);
}
