using Microsoft.Extensions.Caching.Distributed;
using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace OeTube.Application.Caches
{
    public interface IAccessCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        bool ConsiderUov { get; set; }
        bool? HideErrors { get; set; }
        TimeSpan? RelativeExpiration { get; set; }

        Task<bool> GetOrAddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task SetManyAsync(IEnumerable<TEntity> entities, bool value, CancellationToken cancellationToken = default);
    }

    public class AccessCacheService<TRepository, TEntity, TKey> : IAccessCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
            where TRepository : IHasAccessRepository<TEntity, TKey>
    {
        public bool? HideErrors { get; set; }
        public bool ConsiderUov { get; set; } = false;
        public TimeSpan? RelativeExpiration { get; set; }

        public AccessCacheService(TRepository repository, ICurrentUser currentUser, IDistributedCache<CacheItem<bool>, CacheKey> cache)
        {
            Repository = repository;
            CurrentUser = currentUser;
            Cache = cache;
        }

        protected TRepository Repository { get; }
        protected ICurrentUser CurrentUser { get; }
        protected IDistributedCache<CacheItem<bool>, CacheKey> Cache { get; }

        protected virtual Func<DistributedCacheEntryOptions> CreateOptionsFactory()
        {
            return () => new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = RelativeExpiration
            };
        }

        protected virtual CacheKey CreateKey(TEntity entity)
        {
            return new CacheKey()
            {
                Key = $"{typeof(TEntity).Name}_{entity.Id}_{CurrentUser.Id}_ACCESS"
            };
        }

        protected virtual Func<Task<CacheItem<bool>>> CreateItemFactory(TEntity entity)
        {
            return async () => new CacheItem<bool>(await Repository.HasAccessAsync(CurrentUser.Id, entity));
        }

        public async Task SetManyAsync(IEnumerable<TEntity> entities, bool value, CancellationToken cancellationToken = default)
        {
            var pairs = entities.ToDictionary(CreateKey, e => new CacheItem<bool>(value));
            await Cache.SetManyAsync(pairs, CreateOptionsFactory()(), HideErrors, ConsiderUov, cancellationToken);
        }

        public async Task<bool> GetOrAddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var key = CreateKey(entity);
            var item = await Cache.GetOrAddAsync(key, CreateItemFactory(entity), CreateOptionsFactory(), HideErrors, ConsiderUov, cancellationToken);
            if (item is not null && item.Value is bool boolean)
            {
                return boolean;
            }
            else
            {
                throw new InvalidCastException();
            }
        }
    }
}