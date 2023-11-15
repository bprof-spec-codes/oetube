using OeTube.Application.Caches.Access;
using OeTube.Application.Caches.DtoCaches;
using OeTube.Application.Caches.Source;
using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace OeTube.Application.Caches.Composite
{
    public interface ICompositeAccessCacheService<TEntity, TKey> :ICompositeCacheService<TEntity,TKey>
        where TEntity : class, IEntity<TKey>
    {
        IAccessCacheService<TEntity, TKey> AccessCacheService { get; }
    }

    public class CompositeAccessCacheService<TAccessRepository, TEntity, TKey> :
        CompositeCacheService<TAccessRepository, TEntity, TKey>, ICompositeAccessCacheService<TEntity, TKey> where TEntity : class, IEntity<TKey>
        where TAccessRepository : IHasAccessRepository<TEntity, TKey>
    {
        public CompositeAccessCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser, ISourceCacheFactory sourceCacheFactory, TAccessRepository repository) : base(serviceProvider, currentUser, sourceCacheFactory, repository)
        {
            AccessCacheService = new AccessCacheService<TAccessRepository, TEntity, TKey>(SourceCache, ServiceProvider, Repository, CurrentUser);
        }

        public IAccessCacheService<TEntity, TKey> AccessCacheService { get; }
    }
}