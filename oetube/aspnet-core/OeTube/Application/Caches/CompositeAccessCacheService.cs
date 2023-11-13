using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace OeTube.Application.Caches
{
    public interface ICompositeCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        IDtoCacheService<TEntity, TKey> GlobalDtoCache { get; }
        IDtoCacheService<TEntity, TKey> RequesterDtoCache { get; }
    }

    public class CompositeCacheService<TEntity, TKey> : ICompositeCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        public CompositeCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser)
        {
            RequesterDtoCache = new RequesterDtoCacheService<TEntity, TKey>(serviceProvider, currentUser);
            GlobalDtoCache = new GlobalDtoCacheService<TEntity, TKey>(serviceProvider);
        }

        public IDtoCacheService<TEntity, TKey> RequesterDtoCache { get; }
        public IDtoCacheService<TEntity, TKey> GlobalDtoCache { get; }
    }

    public class GroupCacheService : CompositeCacheService<Group, Guid>, ITransientDependency
    {
        public GroupCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser) : base(serviceProvider, currentUser)
        {
        }
    }

    public class UserCacheService : CompositeCacheService<OeTubeUser, Guid>, ITransientDependency
    {
        public UserCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser) : base(serviceProvider, currentUser)
        {
        }
    }

    public interface ICompositeAccessCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        IAccessCacheService<TEntity, TKey> AccessCacheService { get; }
    }

    public class CompositeAccessCacheService<TAccessRepository, TEntity, TKey> : CompositeCacheService<TEntity, TKey>, ICompositeAccessCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
            where TAccessRepository : IHasAccessRepository<TEntity, TKey>
    {
        public CompositeAccessCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser, IDistributedCache<CacheItem<bool>, CacheKey> cache, TAccessRepository repository) : base(serviceProvider, currentUser)
        {
            AccessCacheService = new AccessCacheService<TAccessRepository, TEntity, TKey>(repository, currentUser, cache);
        }

        public IAccessCacheService<TEntity, TKey> AccessCacheService { get; }
    }

    public class VideoCacheService : CompositeAccessCacheService<IVideoRepository, Video, Guid>, ITransientDependency
    {
        public VideoCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser, IDistributedCache<CacheItem<bool>, CacheKey> cache, IVideoRepository repository) : base(serviceProvider, currentUser, cache, repository)
        {
        }
    }

    public class PlaylistCacheService : CompositeAccessCacheService<IPlaylistRepository, Playlist, Guid>, ITransientDependency
    {
        public PlaylistCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser, IDistributedCache<CacheItem<bool>, CacheKey> cache, IPlaylistRepository repository) : base(serviceProvider, currentUser, cache, repository)
        {
        }
    }
}