using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Caches.DtoCaches
{
    public class DtoCacheConfiguration<TEntity, TKey>
        where TEntity : class, IEntity<TKey>

    {
        public DtoCacheConfiguration(CacheValueFactory<TEntity, TKey> factoryMethod,
                                 TimeSpan relativeExpiration)
        {
            RelativeExpiration = relativeExpiration;
            FactoryMethod = factoryMethod;
        }

        public TimeSpan RelativeExpiration { get; }
        public CacheValueFactory<TEntity, TKey> FactoryMethod { get; }
    }
}