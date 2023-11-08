using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IParentReadRepository<TParentEntity,TChildEntity,TChildQueryArgs>
        where TParentEntity : class, IEntity
        where TChildEntity : class, IEntity
        where TChildQueryArgs : IQueryArgs
    {
        public Task<PaginationResult<TChildEntity>> GetChildEntitiesAsync(TParentEntity entity,
                                                          TChildQueryArgs? args = default,
                                                          bool includeDetails = false,
                                                          CancellationToken cancellationToken = default);
    }

}