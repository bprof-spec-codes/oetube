using Volo.Abp.Caching;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Caches
{
    public class GlobalDtoCacheService<TEntity, TKey> : DtoCacheService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public GlobalDtoCacheService(IAbpLazyServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Func<Task<CacheItem<TValue>>> CreateItemFactory<TDto,TValue>(TKey key,TEntity? entity, Expression<PropertySelector<TDto,TValue>> propertySelector)
        {
            var config = Configurations[CreateConfigKey(propertySelector)];
            return async () => new CacheItem<TValue>((TValue?)await config.FactoryMethod(key, entity, null));
        }

        protected override CacheKey CreateKey<TDto,TValue>(TKey key,TEntity? entity, Expression<PropertySelector<TDto,TValue>> propertySelector)
        {
            return new CacheKey()
            {
                Key = $"{typeof(TDto).Name}-{typeof(TEntity).Name}-{key}-{ExpressionToString(propertySelector)}"
            };
        }
    }

}
