using OeTube.Application.Caches;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Url;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Repositories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Dtos.Playlists
{
    public class PlaylistMapper : AsyncNewDestinationObjectMapper<Playlist, PlaylistDto>, ITransientDependency
    {
        private readonly CreatorDtoMapper _creatorMapper;
        private readonly PlaylistUrlService _urlService;
        private readonly PlaylistCacheService _playlistCache;

        public PlaylistMapper(CreatorDtoMapper creatorMapper, PlaylistCacheService playlistCache, IPlaylistRepository repository, PlaylistUrlService urlService)
        {
            _creatorMapper = creatorMapper;
            _playlistCache = playlistCache;
            _playlistCache.ConfigureTotalDuration(repository);
            _urlService = urlService;
        }

        public override async Task<PlaylistDto> MapAsync(Playlist source, PlaylistDto destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.CreationTime = source.CreationTime;
            destination.Description = source.Description;
            destination.Items = source.Items.Select(i => i.VideoId).ToList();
            destination.Image = _urlService.GetImageUrl(source.Id);
            destination.Creator = await _creatorMapper.MapAsync(source.CreatorId);
            destination.TotalDuration = await _playlistCache.GetOrAddTotalDurationAsync(source);
            return destination;
        }
    }

    public static class PlaylistDtoCacheExtension
    {
        public static PlaylistCacheService ConfigureTotalDuration(this PlaylistCacheService cacheService, IPlaylistRepository repository)
        {
            cacheService.RequesterDtoCache.ConfigureProperty<PlaylistDto, TimeSpan>
                (p => p.TotalDuration, async (key, entity, userId) => await repository.GetAvaliableTotalDurationAsync(userId, entity!), TimeSpan.FromMinutes(5));
            return cacheService;
        }

        public static async Task<TimeSpan> GetOrAddTotalDurationAsync(this PlaylistCacheService cacheService, Playlist entity)
        {
            return await cacheService.RequesterDtoCache.GetOrAddAsync<PlaylistDto, TimeSpan>(entity, p => p.TotalDuration);
        }

        public static async Task DeleteTotalDurationAsync(this PlaylistCacheService cacheService, Playlist entity)
        {
            await cacheService.RequesterDtoCache.DeleteAsync<PlaylistDto, TimeSpan>(entity, p => p.TotalDuration);
        }
    }

    public class PlaylistDto : EntityDto<Guid>, IMayHaveCreatorDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreationTime { get; set; }
        public List<Guid> Items { get; set; } = new List<Guid>();
        public string? Image { get; set; }
        public CreatorDto? Creator { get; set; }
        public TimeSpan TotalDuration { get; set; }
    }
}