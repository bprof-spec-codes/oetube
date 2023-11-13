using OeTube.Application.Caches;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Url;
using OeTube.Domain.Entities.Playlists;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Dtos.Playlists
{
    public class PlaylistItemMapper : AsyncNewDestinationObjectMapper<Playlist, PlaylistItemDto>, ITransientDependency
    {
        private readonly PlaylistUrlService _urlService;
        private readonly CreatorDtoMapper _creatorMapper;
        private readonly PlaylistCacheService _cacheService;

        public PlaylistItemMapper(PlaylistUrlService urlService, CreatorDtoMapper creatorMapper, PlaylistCacheService cacheService)
        {
            _urlService = urlService;
            _creatorMapper = creatorMapper;
            _cacheService = cacheService;
        }

        public override async Task<PlaylistItemDto> MapAsync(Playlist source, PlaylistItemDto destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.CreationTime = source.CreationTime;
            destination.ThumbnailImage = _urlService.GetThumbnailImageUrl(source.Id);
            destination.Creator = await _creatorMapper.MapAsync(source.CreatorId);
            destination.TotalDuration = await _cacheService.GetOrAddTotalDurationAsync(source);
            return destination;
        }
    }

    public class PlaylistItemDto : EntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreationTime { get; set; }
        public string? ThumbnailImage { get; set; }
        public CreatorDto? Creator { get; set; }
        public TimeSpan TotalDuration { get; set; }
    }
}