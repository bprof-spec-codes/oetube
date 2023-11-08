using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Services.Url;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.Videos
{
    public class VideoListItemMapper : IObjectMapper<Video, VideoListItemDto>, ITransientDependency
    {
        private readonly IVideoUrlService _videoUrlService;
        private readonly IObjectMapper<Guid?, CreatorDto?> _creatorMapper;

        public VideoListItemMapper(IVideoUrlService videoUrlService, IObjectMapper<Guid?, CreatorDto?> creatorMapper)
        {
            this._videoUrlService = videoUrlService;
            _creatorMapper = creatorMapper;
        }

        public VideoListItemDto Map(Video source)
        {
            return Map(source, new VideoListItemDto());
        }

        public VideoListItemDto Map(Video source, VideoListItemDto destination)
        {
            destination.Id = source.Id;
            destination.CreationTime = source.CreationTime;
            destination.Duration = source.Duration;
            destination.Name = source.Name;
            destination.PlaylistId = null;
            destination.IndexImage = _videoUrlService.GetIndexImageUrl(source.Id);
            destination.Creator = _creatorMapper.Map(source.CreatorId);
            return destination;
        }
    }

    public class VideoListItemDto:EntityDto<Guid>,IMayHaveCreatorDto
    {
        public string Name { get; set; } = string.Empty;
        public string? IndexImage { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? PlaylistId { get; set; }
        public CreatorDto? Creator { get; set; }

    }
}