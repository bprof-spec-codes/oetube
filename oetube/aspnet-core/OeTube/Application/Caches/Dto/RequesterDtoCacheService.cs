using System.Linq.Expressions;
using OeTube.Application.Caches.Source;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace OeTube.Application.Caches.DtoCaches
{
    public interface IRequesterDtoCacheService<TEntity,TKey>:IDtoCacheService<TEntity,TKey>
        where TEntity : class, IEntity<TKey>
    { }
    public class RequesterDtoCacheService<TEntity, TKey> : DtoCacheService<TEntity, TKey>,
        IRequesterDtoCacheService<TEntity,TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected ICurrentUser CurrentUser { get; }
        public RequesterDtoCacheService(ISourceCacheService<TKey> sourceCache, IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser) : base(sourceCache, serviceProvider)
        {
            CurrentUser = currentUser;
        }

        protected override async Task<CacheItem<TValue>> CreateItemAsync<TDto, TValue>(TKey key, TEntity? entity, int? sourceCheckSum, Expression<PropertySelector<TDto, TValue>> propertySelector) 
        {
            var config = Configurations[CreateConfigKey(propertySelector)];
            var value = (TValue?)await config.FactoryMethod(key, entity, CurrentUser.Id);
            return new CacheItem<TValue>(value, sourceCheckSum);
        }


        protected override CacheKey CreateCacheKey<TDto, TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto, TValue>> propertySelector)
        {
            return new CacheKey($"{typeof(TDto).Name}-{typeof(TEntity).Name}-{key}-{ExpressionToString(propertySelector)}-{CurrentUser.Id?.ToString() ?? "null"}");
        }
    }
}