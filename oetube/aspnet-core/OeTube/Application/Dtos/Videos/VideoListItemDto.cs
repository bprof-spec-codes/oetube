using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Url;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Dtos.Videos
{
    public class VideoListItemMapper : AsyncNewDestinationObjectMapper<Video, VideoListItemDto>, ITransientDependency
    {
        private readonly VideoUrlService _videoUrlService;
        private readonly CreatorDtoMapper _creatorMapper;

        public VideoListItemMapper(VideoUrlService videoUrlService, CreatorDtoMapper creatorMapper)
        {
            _videoUrlService = videoUrlService;
            _creatorMapper = creatorMapper;
        }

        public override async Task<VideoListItemDto> MapAsync(Video source, VideoListItemDto destination)
        {
            destination.Id = source.Id;
            destination.CreationTime = source.CreationTime;
            destination.Duration = source.Duration;
            destination.Name = source.Name;
            destination.PlaylistId = null;
            destination.IndexImage = _videoUrlService.GetIndexImageUrl(source.Id);
            destination.Creator = await _creatorMapper.MapAsync(source.CreatorId);
            return destination;
        }
    }

    public class VideoListItemDto : EntityDto<Guid>, IMayHaveCreatorDto
    {
        public string Name { get; set; } = string.Empty;
        public string? IndexImage { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? PlaylistId { get; set; }
        public CreatorDto? Creator { get; set; }
    }
}