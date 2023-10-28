using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Managers;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace OeTube.Workers
{
    public class PeriodicDeleteUncompletedVideos : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly TimeSpan period = TimeSpan.FromHours(4);

        public PeriodicDeleteUncompletedVideos(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
        {
            Timer.Period = (int)period.TotalMilliseconds;
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Starting: " + GetType().Name);

            var videoManager = workerContext
                .ServiceProvider
                .GetRequiredService<VideoManager>();
            
            List<Video> videos;
            var args = new QueryArgs()
            {
                MaxResultCount = 100
            };

            do
            {
                videos = await videoManager.GetUncompletedVideosAsync(period, args);
                await videoManager.DeleteManyAsync(videos, true);
            } while (videos.Count != 0);


            Logger.LogInformation("Completed: " + GetType().Name);
        }
    }
}