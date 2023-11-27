using System.Linq.Expressions;
using OeTube.Application.Caches.Source;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Caches.DtoCaches
{
    public interface IGlobalDtoCacheService<TEntity,TKey>: IDtoCacheService<TEntity, TKey> 
        where TEntity:class,IEntity<TKey>
    { }

    public class GlobalDtoCacheService<TEntity, TKey> : DtoCacheService<TEntity, TKey>,IGlobalDtoCacheService<TEntity,TKey>
        where TEntity : class, IEntity<TKey>
    {
        public GlobalDtoCacheService(ISourceCacheService<TKey> sourceCache, IAbpLazyServiceProvider serviceProvider) : base(sourceCache, serviceProvider)
        {
        }

        protected override async Task<CacheItem<TValue>> CreateItemAsync<TDto, TValue>(TKey key, TEntity? entity, int? sourceCheckSum, Expression<PropertySelector<TDto, TValue>> propertySelector)
        {
            var config = Configurations[CreateConfigKey(propertySelector)];
            var value =(TValue?) await config.FactoryMethod(key, entity, null);
            return new CacheItem<TValue>(value, sourceCheckSum);
        }

        protected override CacheKey CreateCacheKey<TDto, TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto, TValue>> propertySelector)
        {
            return new CacheKey($"{typeof(TDto).Name}-{typeof(TEntity).Name}-{key}-{ExpressionToString(propertySelector)}");
        }
    }
}