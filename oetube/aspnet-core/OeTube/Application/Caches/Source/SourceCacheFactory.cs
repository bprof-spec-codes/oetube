using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Caches.Source
{
    public interface ISourceCacheFactory
    {
        ISourceCacheService<TKey> Create<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>;
    }

    public class SourceCacheFactory : ISourceCacheFactory,ITransientDependency
    {
        private readonly IDistributedCache<CacheSourceItem, CacheKey> SourceCache;

        public SourceCacheFactory(IDistributedCache<CacheSourceItem, CacheKey> sourceCache)
        {
            SourceCache = sourceCache;
        }
        public ISourceCacheService<TKey> Create<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
        {
            return new SourceCacheService<TEntity, TKey>(SourceCache);
        }
    }
}