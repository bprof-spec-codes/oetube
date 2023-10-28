using OeTube.Domain.Managers;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace OeTube.Workers
{
    public class PeriodicDeleteUncompletedVideos : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly TimeSpan period = TimeSpan.FromHours(6);

        public PeriodicDeleteUncompletedVideos(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
        {
            Timer.Period = period.Milliseconds;
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Starting: " + GetType().Name);

            var videoManager = workerContext
                .ServiceProvider
                .GetRequiredService<VideoManager>();

            await videoManager.DeleteUncompletedVideosAsync(period);

            Logger.LogInformation("Completed: " + GetType().Name);
        }
    }
}