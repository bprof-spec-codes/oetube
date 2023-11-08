using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IHasAccessibilityRepository<TEntity,TKey,TQueryArgs>
        where TEntity:class,IEntity<Guid>
        where TQueryArgs:IQueryArgs
    {
        Task<bool> HasAccessAsync(Guid? requesterId, TEntity entity);
        Task<PaginationResult<TEntity>> GetAvaliableAsync(Guid? requesterId,
                                              TQueryArgs? args = default,
                                              bool includeDetails = false,
                                              CancellationToken cancellationToken = default);
    }
}