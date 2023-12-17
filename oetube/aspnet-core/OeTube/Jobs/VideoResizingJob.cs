using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FileHandlers;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace OeTube.Jobs
{
    public class VideoResizingJob : AsyncBackgroundJob<VideoResizingArgs>, ITransientDependency
    {
        private readonly IVideoResizingHandler _resizingHandler;

        public VideoResizingJob(IVideoResizingHandler resizingHandler)
        {
            _resizingHandler = resizingHandler;
        }

        public override async Task ExecuteAsync(VideoResizingArgs args)
        {
            await _resizingHandler.HandleFileAsync<Video>(args);
        }
    }
}