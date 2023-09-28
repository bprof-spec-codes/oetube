using Volo.Abp.EventBus;

namespace OeTube.Infrastructure.FFmpeg.Job
{
    [EventName(nameof(FFmpegJobCommandCompletedEto))]
    public record FFmpegJobCommandCompletedEto(Guid JobId, FFmpegResult Result, int CompletedCommandCount, int TotalCommand);
}
