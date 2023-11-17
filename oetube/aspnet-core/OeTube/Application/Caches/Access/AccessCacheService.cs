using Microsoft.Extensions.Caching.Distributed;
using OeTube.Application.Caches.Source;
using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace OeTube.Application.Caches.Access
{
    public interface IAccessCacheService<TEntity, TKey>:ISourceCacheService<TKey> where TEntity : class, IEntity<TKey>
    {
        TimeSpan RelativeExpiration { get; set; }

        Task<bool> GetOrAddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task SetManyAsync(IEnumerable<TEntity> entities, bool value, CancellationToken cancellationToken = default);
    }

    public class AccessCacheService<TRepository, TEntity, TKey> :
            CacheService<TEntity, TKey>, IAccessCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
            where TRepository : IHasAccessRepository<TEntity, TKey>
    {
        public AccessCacheService(ISourceCacheService<TKey> sourceCache, IAbpLazyServiceProvider serviceProvider, TRepository repository, ICurrentUser currentUser) : base(sourceCache, serviceProvider)
        {
            Repository = repository;
            CurrentUser = currentUser;
        }

        protected TRepository Repository { get; }
        protected ICurrentUser CurrentUser { get; }
        public virtual TimeSpan RelativeExpiration { get; set; } = TimeSpan.FromMinutes(10);

        protected virtual CacheKey CreateKey(TEntity entity)
        {
            return new CacheKey($"{typeof(TEntity).Name}_{entity.Id}_{CurrentUser.Id}_ACCESS");
        }

        protected virtual async Task<CacheItem<bool>> CreateItem(TEntity entity, int? sourceCheckSum)
        {
            return new CacheItem<bool>(await Repository.HasAccessAsync(CurrentUser.Id, entity), sourceCheckSum);
        }

        public async Task SetManyAsync(IEnumerable<TEntity> entities, bool value, CancellationToken cancellationToken = default)
        {
            Dictionary<CacheKey, CacheItem<bool>> dict = new Dictionary<CacheKey, CacheItem<bool>>();
            foreach (var item in entities)
            {
                if (cancellationToken.IsCancellationRequested) break;
                var cacheKey = CreateKey(item);
                var cacheItem = new CacheItem<bool>(value, await SourceCache.GetSourceCheckSumAsync(item.Id, cancellationToken));
                dict.Add(cacheKey, cacheItem);
            }
            var cache = GetDistributedCache<bool>();
            await cache.SetManyAsync(dict,
                                     new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = RelativeExpiration },
                                     HideErrors,
                                     ConsiderUow,
                                     cancellationToken);
        }

        public async Task<bool> GetOrAddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return await GetOrAddAsync(entity.Id,
                                       entity,
                                       CreateKey(entity),
                                       (checkSum) => CreateItem(entity, checkSum),
                                       RelativeExpiration,
                                       cancellationToken);
        }
    }
}