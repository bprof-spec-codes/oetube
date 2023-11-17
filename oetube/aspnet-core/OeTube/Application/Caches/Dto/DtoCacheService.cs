using OeTube.Application.Caches.Source;
using System.Linq.Expressions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Caches.DtoCaches
{
    public delegate TValue PropertySelector<TDto, TValue>(TDto dto);
    public delegate Task<object?> CacheValueFactory<TEntity, TKey>(TKey entityKey, TEntity? entity, Guid? requesterId);
    public interface IDtoCacheService<TEntity, TKey>:
        ISourceCacheService<TKey>
        where TEntity : class, IEntity<TKey>
    {
        bool ConsiderUow { get; set; }
        bool? HideErrors { get; set; }
        TimeSpan DefaultRelativeExpiration { get; set; }

        void ConfigureProperty<TDto, TValue>(Expression<PropertySelector<TDto, TValue>> propertySelector, CacheValueFactory<TEntity, TKey> factoryMethod, TimeSpan? relativeExpiration=null);
        Task<TValue?> GetOrAddAsync<TDto, TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto, TValue>> propertySelector, CancellationToken cancellationToken = default);
    }
    public abstract class DtoCacheService<TEntity, TKey> : CacheService<TEntity, TKey>,
        IDtoCacheService<TEntity,TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected Dictionary<string, DtoCacheConfiguration<TEntity, TKey>> Configurations { get; } = new();
        public virtual TimeSpan DefaultRelativeExpiration { get; set; } = TimeSpan.FromMinutes(10);
        protected DtoCacheService(ISourceCacheService<TKey> sourceCache, IAbpLazyServiceProvider serviceProvider) : base(sourceCache, serviceProvider)
        {
        }
        protected virtual string ExpressionToString<TDto, TValue>(Expression<PropertySelector<TDto, TValue>> propertySelector)
        {
            return ((MemberExpression)propertySelector.Body).Member.Name;
        }
        protected virtual string CreateConfigKey<TDto, TValue>(Expression<PropertySelector<TDto, TValue>> propertySelector)
        {
            return $"{typeof(TDto).Name}_{ExpressionToString(propertySelector)}";
        }
        protected abstract CacheKey CreateCacheKey<TDto, TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto, TValue>> propertySelector);
        protected abstract Task<CacheItem<TValue>> CreateItemAsync<TDto, TValue>(TKey key, TEntity? entity, int? sourceCheckSum, Expression<PropertySelector<TDto, TValue>> propertySelector);
        public virtual void ConfigureProperty<TDto, TValue>(Expression<PropertySelector<TDto, TValue>> propertySelector, CacheValueFactory<TEntity, TKey> factoryMethod, TimeSpan? relativeExpiration = null)
        {
            relativeExpiration ??= DefaultRelativeExpiration;
            Configurations[CreateConfigKey(propertySelector)] = new DtoCacheConfiguration<TEntity, TKey>(factoryMethod, relativeExpiration??DefaultRelativeExpiration);
        }

        public virtual async Task<TValue?> GetOrAddAsync<TDto, TValue>(TKey key, TEntity? entity, Expression<PropertySelector<TDto, TValue>> propertySelector, CancellationToken cancellationToken = default)
        {
            return await GetOrAddAsync(key,
                                       entity,
                                       CreateCacheKey(key, entity, propertySelector),
                                       (checkSum) => CreateItemAsync(key, entity, checkSum, propertySelector),
                                       Configurations[CreateConfigKey(propertySelector)].RelativeExpiration,
                                       cancellationToken);
        }
    }
}