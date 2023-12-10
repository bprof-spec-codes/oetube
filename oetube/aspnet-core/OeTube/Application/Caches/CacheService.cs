using Microsoft.Extensions.Caching.Distributed;
using OeTube.Application.Caches.Source;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Caches
{
    public delegate Task<CacheItem<TValue>> CacheItemFactory<TValue>(int? checkSum);
    public abstract class CacheService<TEntity, TKey>:ISourceCacheService<TKey>
    {
        protected CacheService(ISourceCacheService<TKey> sourceCache, IAbpLazyServiceProvider serviceProvider)
        {
            SourceCache = sourceCache;
            ServiceProvider = serviceProvider;
        }

        public virtual bool? HideErrors { get; set; }
        public virtual bool ConsiderUow { get; set; } = false;

        protected ISourceCacheService<TKey> SourceCache { get; }
        protected IAbpLazyServiceProvider ServiceProvider { get; }

        Type ISourceCacheService<TKey>.EntityType => SourceCache.EntityType;

        public Task<int?> GetSourceCheckSumAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return SourceCache.GetSourceCheckSumAsync(id, cancellationToken);
        }

        public Task RefreshSourceAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return SourceCache.RefreshSourceAsync(id, cancellationToken);
        }

        protected virtual IDistributedCache<CacheItem<TValue>, CacheKey> GetDistributedCache<TValue>()
        {
            return ServiceProvider.LazyGetRequiredService<IDistributedCache<CacheItem<TValue>, CacheKey>>();
        }

        protected virtual async Task<TValue?> GetOrAddAsync<TValue>(TKey key,
                                                                    TEntity? entity,
                                                                    CacheKey cacheKey,
                                                                    CacheItemFactory<TValue> itemFactory,
                                                                    TimeSpan relativeExpiration,
                                                                    CancellationToken cancellationToken)
        {
            var checksum = await SourceCache.GetSourceCheckSumAsync(key, cancellationToken);
            var cache = GetDistributedCache<TValue>();
            var cacheItem = await cache.GetAsync(cacheKey, HideErrors, ConsiderUow, cancellationToken);
            if (cacheItem is null || checksum != cacheItem.SourceCheckSum)
            {
                cacheItem = await itemFactory(checksum);
                await cache.SetAsync(cacheKey,
                                      cacheItem,
                                      new DistributedCacheEntryOptions()
                                      { AbsoluteExpirationRelativeToNow = relativeExpiration },
                                      HideErrors,
                                      ConsiderUow,
                                      cancellationToken);
            }
            return cacheItem.Value;
        }
    }
}