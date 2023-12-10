using OeTube.Application.Caches.Access;
using OeTube.Application.Caches.DtoCaches;
using OeTube.Application.Caches.Source;
using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace OeTube.Application.Caches.Composite
{
    public interface ICompositeCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        IGlobalDtoCacheService<TEntity, TKey> GlobalDtoCache { get; }
        IRequesterDtoCacheService<TEntity, TKey> RequesterDtoCache { get; }
        ISourceCacheService<TKey> SourceCache { get; }
    }

    public class CompositeCacheService<TRepository, TEntity, TKey> : ICompositeCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
            where TRepository : IReadRepository<TEntity, TKey>
    {

        public CompositeCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser, ISourceCacheFactory sourceCacheFactory, TRepository repository)
        {
            ServiceProvider = serviceProvider;
            CurrentUser = currentUser;
            SourceCache = sourceCacheFactory.Create<TEntity, TKey>();
            RequesterDtoCache = new RequesterDtoCacheService<TEntity, TKey>(SourceCache, ServiceProvider, CurrentUser);
            GlobalDtoCache = new GlobalDtoCacheService<TEntity, TKey>(SourceCache, ServiceProvider);
            Repository = repository;
        }

        protected TRepository Repository { get; }
        protected IAbpLazyServiceProvider ServiceProvider { get; }
        protected ICurrentUser CurrentUser { get; }
        public IRequesterDtoCacheService<TEntity, TKey> RequesterDtoCache { get; }
        public IGlobalDtoCacheService<TEntity, TKey> GlobalDtoCache { get; }
        public ISourceCacheService<TKey> SourceCache { get; }
    }
}