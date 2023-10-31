using OeTube.Domain.Entities.Videos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace OeTube.Events
{
    public class VideoUploadCompletedEto
    {
        public Video Video { get; }

        public VideoUploadCompletedEto(Video video)
        {
            Video = video;
        }
    }

    public class VideoUploadCompletedEventHandler : ILocalEventHandler<VideoUploadCompletedEto>, ITransientDependency
    {
        public Task HandleEventAsync(VideoUploadCompletedEto eventData)
        {
            return Task.CompletedTask;
        }
    }
}