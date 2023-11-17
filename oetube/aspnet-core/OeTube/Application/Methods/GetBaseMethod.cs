using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods
{
    public abstract class GetBaseMethod<TEntity, TKey> : ApplicationMethod
         where TEntity : class, IEntity<TKey>
    {
        protected GetBaseMethod(IAbpLazyServiceProvider serviceProvider,
                                IReadRepository<TEntity, TKey> repository) : base(serviceProvider)
        {
            Repository = repository;
        }

        protected virtual IReadRepository<TEntity, TKey> Repository { get; }

        protected virtual async Task<TEntity> GetByIdAsync(TKey id, bool includeDetails = true)
        {
            return await Repository.GetAsync(id, includeDetails);
        }
    }
}