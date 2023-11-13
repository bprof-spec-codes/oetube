using OeTube.Domain.Repositories;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities;
using System.Linq.Expressions;
using Volo.Abp.TenantManagement;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Localization.VirtualFiles;
using Newtonsoft.Json.Linq;
using Autofac.Core;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using System.Diagnostics;

namespace OeTube.Application.Caches
{

  
    public delegate TValue PropertySelector<TDto,TValue>(TDto dto);
    public delegate Task<object?> CacheValueFactory<TEntity,TKey>(TKey entityKey, TEntity? entity, Guid? requesterId);

    public interface IDtoCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        bool ConsiderUov { get; set; }
        bool? HideErrors { get; set; }

        void ConfigureProperty<TDto,TValue>(Expression<PropertySelector<TDto,TValue>> propertySelector, CacheValueFactory<TEntity, TKey> factoryMethod, TimeSpan? relativeExpiration);
        Task DeleteAsync<TDto,TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default);
        Task DeleteAsync<TDto,TValue>(TEntity entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default);
        Task<TValue> GetOrAddAsync<TDto, TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default);
        Task<TValue> GetOrAddAsync<TDto, TValue>(TEntity entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default);
        Task SetAsync<TDto,TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default);
        Task SetAsync<TDto,TValue>(TEntity entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default);
    }

    public abstract class DtoCacheService<TEntity, TKey> : IDtoCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        public bool? HideErrors { get; set; }
        public bool ConsiderUov { get; set; } = false;
        protected Dictionary<string, DtoCacheConfiguration<TEntity, TKey>> Configurations { get; } = new();
        protected IAbpLazyServiceProvider ServiceProvider { get; }
        protected DtoCacheService(IAbpLazyServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        delegate T MyFunc<in K,out T> (K t);
        protected virtual string ExpressionToString<TDto,TValue>(Expression<PropertySelector<TDto,TValue>> propertySelector)
        {
            MyFunc<int,object> t;
            MyFunc<int, string> x= (i)=>i.ToString();
            t = x;

            try
            {
                return ((MemberExpression)propertySelector.Body).Member.Name;

            }
            catch
            {
                return string.Empty;
            }
        }
        protected virtual IDistributedCache<CacheItem<TValue>,CacheKey> GetDistributedCache<TValue>()
        {
            return ServiceProvider.LazyGetRequiredService<IDistributedCache<CacheItem<TValue>, CacheKey>>();
        }
        public virtual void ConfigureProperty<TDto,TValue>(Expression<PropertySelector<TDto,TValue>> propertySelector, CacheValueFactory<TEntity, TKey> factoryMethod, TimeSpan? relativeExpiration)
        {
            Configurations[CreateConfigKey(propertySelector)] = new DtoCacheConfiguration<TEntity, TKey>(factoryMethod, relativeExpiration);
        }
        protected virtual string CreateConfigKey<TDto,TValue>(Expression<PropertySelector<TDto,TValue>> propertySelector)
        {
            return $"{typeof(TDto).Name}_{ExpressionToString(propertySelector)}";
        }
        protected abstract CacheKey CreateKey<TDto,TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto,TValue>> propertySelector);
        protected abstract Func<Task<CacheItem<TValue>>> CreateItemFactory<TDto,TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto,TValue>> propertySelector);
        protected virtual Func<DistributedCacheEntryOptions> CreateOptionsFactory<TDto,TValue>(Expression<PropertySelector<TDto,TValue>> propertySelector)
        {
            return () => new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = Configurations[CreateConfigKey(propertySelector)].RelativeExpiration,
            };
        }
        public async Task<TValue> GetOrAddAsync<TDto, TValue>(TEntity entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default)
        {
            return await GetOrAddAsync<TDto,TValue>(entity.Id, entity, propertySelector, cancellationToken);
        }
        public virtual async Task<TValue> GetOrAddAsync<TDto, TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheItem = await GetDistributedCache<TValue>().GetOrAddAsync(CreateKey(key, entity, propertySelector),
                                                   CreateItemFactory(key, entity, propertySelector),
                                                   CreateOptionsFactory(propertySelector),
                                                   HideErrors,
                                                   ConsiderUov,
                                                   cancellationToken);
                return cacheItem!.Value;
            }
            catch(InvalidCastException ex)
            {
                throw;
            }
           
        }
        public async Task SetAsync<TDto,TValue>(TEntity entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default)
        {
                 await SetAsync(entity.Id, entity, propertySelector, cancellationToken);
        }
        public virtual async Task SetAsync<TDto,TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default)
        {
            var cacheKey = CreateKey(key, entity, propertySelector);
            var value = await CreateItemFactory(key, entity, propertySelector)();
            await GetDistributedCache<TValue>().SetAsync(cacheKey, value, CreateOptionsFactory(propertySelector)(), HideErrors, ConsiderUov, cancellationToken);
        }
        public  async Task DeleteAsync<TDto,TValue>(TEntity entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(entity.Id, entity, propertySelector, cancellationToken);
        }
        public virtual async Task DeleteAsync<TDto,TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto,TValue>> propertySelector, CancellationToken cancellationToken = default)
        {
            var cacheKey = CreateKey(key, entity, propertySelector);
            await GetDistributedCache<TValue>().RemoveAsync(cacheKey, HideErrors, ConsiderUov, cancellationToken);
        }
    }

}
