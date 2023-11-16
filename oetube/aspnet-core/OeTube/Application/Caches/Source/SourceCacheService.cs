using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;
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
        protected IDistributedCache<SourceCacheItem, CacheKey> SourceCache { get; }
        public Type EntityType { get; }
        public SourceCacheService(IDistributedCache<SourceCacheItem, CacheKey> sourceCache)
        {
            EntityType = typeof(TEntity);
            SourceCache = sourceCache;
        }
        protected virtual CacheKey CreateKey(TKey id)
        {
            return new CacheKey($"{EntityType.Name}_{id}");
        }

        public virtual async Task<int?> GetSourceCheckSumAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var key = CreateKey(id);
            var item = await SourceCache.GetAsync(key, false, false, cancellationToken);
            return item?.CheckSum;
        }
        public virtual async Task RefreshSourceAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var key = CreateKey(id);
            var item = new SourceCacheItem(DateTime.Now.GetHashCode());
            await SourceCache.SetAsync(key, item, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = RelativeExpiration
            }, false, false, cancellationToken);
        }
    }
}