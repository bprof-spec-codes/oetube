using OeTube.Infrastructure.VideoStorage;
using System.Diagnostics;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;

namespace OeTube.Infrastructure.FFmpeg.Job
{

    public class FFmpegJob : AsyncBackgroundJob<FFmpegJobArgs>, ITransientDependency
    {
        private readonly IFFmpegService _ffmpeg;
        private readonly IDistributedEventBus _distributedEventBus;

        public FFmpegJob(IFFmpegService ffmpeg, IDistributedEventBus distributedEventBus)
        {
            _ffmpeg = ffmpeg;
            _distributedEventBus = distributedEventBus;
        }
    
        public async override Task ExecuteAsync(FFmpegJobArgs args)
        {
            int completedCommand = 0;
            foreach (var item in args.FFmpegCommands)
            {
                var result=await _ffmpeg.ProcessAsync(item, null);
                completedCommand++;
                await _distributedEventBus.PublishAsync(new FFmpegJobCommandCompletedEto(args.JobId, result, completedCommand, args.FFmpegCommands.Length));
            }
        }
    }
}
