using Volo.Abp.Caching;
using Volo.Abp.Users;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Caches
{
    public class RequesterDtoCacheService<TEntity, TKey> : DtoCacheService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected ICurrentUser CurrentUser { get; }

        public RequesterDtoCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser) : base(serviceProvider)
        {
            CurrentUser = currentUser;
        }

        protected override Func<Task<CacheItem<TValue>>> CreateItemFactory<TDto, TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto, TValue>> propertySelector)
        {
            var config = Configurations[CreateConfigKey(propertySelector)];
            return async () => new CacheItem<TValue>((TValue)await config.FactoryMethod(key, entity, CurrentUser.Id));
        }
        protected override CacheKey CreateKey<TDto,TValue>(TKey key,TEntity? entity, Expression<PropertySelector<TDto,TValue>> propertySelector)
        {
            return new CacheKey()
            {
                Key = $"{typeof(TDto).Name}-{typeof(TEntity).Name}-{key}-{ExpressionToString(propertySelector)}-{CurrentUser.Id?.ToString() ?? "null"}"
            };
        }
    }
}

