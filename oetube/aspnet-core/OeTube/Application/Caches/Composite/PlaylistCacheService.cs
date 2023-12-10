using OeTube.Application.Caches.Composite;
using OeTube.Application.Caches.Source;
using OeTube.Application.Dtos.Playlists;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Repositories;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace OeTube.Application.Caches
{
    public class PlaylistCacheService : CompositeAccessCacheService<IPlaylistRepository, Playlist, Guid>, ITransientDependency
    {
        public PlaylistCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser, ISourceCacheFactory sourceCacheFactory, IPlaylistRepository repository) : base(serviceProvider, currentUser, sourceCacheFactory, repository)
        {
            ConfigureTotalDuration();
            ConfigureItemsCount();
        }
        protected virtual void ConfigureItemsCount()
        {
            RequesterDtoCache.ConfigureProperty<PlaylistItemDto, int>
                (p => p.ItemsCount, async (key, entity, userId) => await Repository.GetAvaliableItemsCountAsync(userId, entity!));
        }
        public async Task<int> GetOrAddItemsCountAsync(Playlist entity)
        {
            return await RequesterDtoCache.GetOrAddAsync<PlaylistItemDto, int>(entity.Id, entity, p => p.ItemsCount);
        }
        protected virtual void ConfigureTotalDuration()
        {
            RequesterDtoCache.ConfigureProperty<PlaylistDto, TimeSpan>
           (p => p.TotalDuration, async (key, entity, userId) => await Repository.GetAvaliableTotalDurationAsync(userId, entity!));
        }
        public async Task<TimeSpan> GetOrAddTotalDurationAsync(Playlist entity)
        {
            return await RequesterDtoCache.GetOrAddAsync<PlaylistDto, TimeSpan>(entity.Id,entity, p => p.TotalDuration);
        }
    }
}