using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
   
    public interface ICreatorRepository<TCreator,TEntity,TQueryArgs>
        where TEntity : class, IEntity
        where TQueryArgs : IQueryArgs
    {
        public Task<PaginationResult<TEntity>> GetCreatedEntitiesAsync(Guid creatorId,
                                                           TQueryArgs? args = default,
                                                           bool includeDetails = false,
                                                           CancellationToken cancellationToken = default);
    }
}