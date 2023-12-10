using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
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

            var repository = workerContext
                .ServiceProvider
                .GetRequiredService<IVideoRepository>();

            PaginationResult<Video> videos;
            var args = new QueryArgs()
            {
                Pagination=new Pagination() { Take=100}
            };
            do
            {
                videos = await repository.GetUncompletedVideosAsync(period, args);
                await repository.DeleteManyAsync(videos.Items, true);
            } while (videos.Items.Count != 0);

            Logger.LogInformation("Completed: " + GetType().Name);
        }
    }
}