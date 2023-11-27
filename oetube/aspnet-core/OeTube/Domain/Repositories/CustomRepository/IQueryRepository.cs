using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IQueryRepository<TEntity, TQueryArgs>
        where TEntity : class, IEntity
        where TQueryArgs : IQueryArgs
    {
        Task<PaginationResult<TEntity>> GetListAsync(TQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);
    }

    public interface IQueryAvaliableRepository<TEntity, TQueryArgs> : IQueryRepository<TEntity, TQueryArgs>
    where TEntity : class, IEntity
    where TQueryArgs : IQueryArgs
    {
        Task<PaginationResult<TEntity>> GetAvaliableAsync(Guid? requesterId,
                                              TQueryArgs? args = default,
                                              bool includeDetails = false,
                                              CancellationToken cancellationToken = default);
    }
}