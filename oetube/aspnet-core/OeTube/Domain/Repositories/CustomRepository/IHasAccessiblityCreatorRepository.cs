using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IHasAccessiblityCreatorRepository<TEntity,TQueryArgs>
        where TEntity : class, IEntity
        where TQueryArgs : IQueryArgs
    {
        Task<PaginationResult<TEntity>> GetAvaliableCreatedEntititesAsync(Guid creatorId,
                                                              Guid? requesterId,
                                                              TQueryArgs? args = default,
                                                              bool includeDetails = false,
                                                              CancellationToken cancellationToken = default);
    }
}