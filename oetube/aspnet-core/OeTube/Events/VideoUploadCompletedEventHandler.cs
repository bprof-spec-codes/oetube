using Microsoft.AspNetCore.SignalR;
using OeTube.Application;
using OeTube.Application.Dtos;
using OeTube.Application.Dtos.Videos;
using OeTube.Domain.Entities.Videos;
using OeTube.Infrastructure.SignalR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectMapping;

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
        readonly private IHubContext<NotifyHub> _hubContext;
        readonly private IAsyncObjectMapper<Video,VideoDto> _objectMapper;
        public VideoUploadCompletedEventHandler(IHubContext<NotifyHub> hubContext, IObjectMapper<Video,VideoDto> objectMapper)
        {
            _hubContext = hubContext;
            _objectMapper = (objectMapper as IAsyncObjectMapper<Video,VideoDto>)!;
        }


        public virtual async Task HandleEventAsync(VideoUploadCompletedEto eventData)
        {
            if (eventData.Video.CreatorId != null)
            {
                var videoDto =await _objectMapper.MapAsync(eventData.Video);
                await _hubContext.Clients.User(eventData.Video.CreatorId.Value.ToString()).SendAsync(NotifyHub.UploadCompleted, videoDto);
            }
        }
    }
}