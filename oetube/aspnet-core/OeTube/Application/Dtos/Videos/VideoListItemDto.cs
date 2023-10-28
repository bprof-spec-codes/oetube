using OeTube.Application.Services.Url;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.Videos
{
    public class VideoListItemMapper : IObjectMapper<Video, VideoListItemDto>, ITransientDependency
    {
        private readonly IVideoUrlService _videoUrlService;

        public VideoListItemMapper(IVideoUrlService videoUrlService)
        {
            this._videoUrlService = videoUrlService;
        }

        public VideoListItemDto Map(Video source)
        {
            return Map(source, new VideoListItemDto());
        }

        public VideoListItemDto Map(Video video, VideoListItemDto destination)
        {
            return new VideoListItemDto()
            {
                Id = video.Id,
                CreationTime = video.CreationTime,
                CreatorId = video.CreatorId,
                Duration = video.Duration,
                Name = video.Name,
                PlaylistId = null,
                IndexImageSource = _videoUrlService.GetIndexImageUrl(video.Id)
            };
        }
    }

    public class VideoListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IndexImageSource { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? PlaylistId { get; set; }
    }
}