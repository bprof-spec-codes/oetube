using Microsoft.Extensions.Caching.Distributed;
using System.Runtime.CompilerServices;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Caches.Source
{
    public interface ISourceCacheService<TKey>
    {
        public Type EntityType { get; }

        Task<int?> GetSourceCheckSumAsync(TKey id, CancellationToken cancellationToken = default);
        Task RefreshSourceAsync(TKey id, CancellationToken cancellationToken = default);
    }
    public class SourceCacheService<TEntity, TKey> : ISourceCacheService<TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected virtual TimeSpan RelativeExpiration { get; } = TimeSpan.FromHours(1);
        protected IDistributedCache<CacheSourceItem, CacheKey> SoureCache { get; }
        public Type EntityType { get; }
        public SourceCacheService(IDistributedCache<CacheSourceItem, CacheKey> sourceCache)
        {
            EntityType = typeof(TEntity);
            SoureCache = sourceCache;
        }
        protected virtual CacheKey CreateKey(TKey id)
        {
            return new CacheKey($"{EntityType.Name}_{id}");
        }

        public virtual async Task<int?> GetSourceCheckSumAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var key = CreateKey(id);
            return (await SoureCache.GetAsync(key, false, false, cancellationToken))?.CheckSum;
        }
        public virtual async Task RefreshSourceAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var key = CreateKey(id);
            var item = new CacheSourceItem();
            await SoureCache.SetAsync(key, item, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = RelativeExpiration
            }, false, false, cancellationToken);
        }
    }
}