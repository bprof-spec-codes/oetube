using OeTube.Infrastructure.FFmpeg.Processes;
using OeTube.Infrastructure.ProcessTemplate;
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
        private readonly FFmpegProcess _ffmpeg;
        private readonly IDistributedEventBus _distributedEventBus;

        public FFmpegJob(FFmpegProcess ffmpeg, IDistributedEventBus distributedEventBus)
        {
            _ffmpeg = ffmpeg;
            _distributedEventBus = distributedEventBus;
        }
    
        public async override Task ExecuteAsync(FFmpegJobArgs args)
        {
            List<ProcessResult> results = new List<ProcessResult>();
            foreach (var item in args.Settings)
            {
                results.Add( await _ffmpeg.StartProcessAsync(item));
            }
            await _distributedEventBus.PublishAsync(new FFmpegJobCompletedEto(args.JobId, results));
        }
    }
}
