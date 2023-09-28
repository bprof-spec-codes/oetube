using System.Diagnostics;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace OeTube.Infrastructure.FFmpeg.Job
{
    public class FFmpegJobCommandCompletedHandler : IDistributedEventHandler<FFmpegJobCommandCompletedEto>, ITransientDependency
    {
        public async Task HandleEventAsync(FFmpegJobCommandCompletedEto eventData)
        {
            Debug.Write(eventData.ToString(), "FFmpeg");
            await Task.CompletedTask;
        }
    }
}
