using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SignalR;
using OeTube.Infrastructure.SignalR;
using System.Diagnostics;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace OeTube.Infrastructure.FFmpeg.Job
{
    public class FFmpegJobCompletedHandler : IDistributedEventHandler<FFmpegJobCompletedEto>, ITransientDependency
    {
        readonly private IHubContext<NotifyHub> _hubContext;

        public FFmpegJobCompletedHandler(IHubContext<NotifyHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleEventAsync(FFmpegJobCompletedEto eventData)
        {
            await _hubContext.Clients.All.SendAsync("video-avaliable",eventData.JobId);
        }
    }
}
