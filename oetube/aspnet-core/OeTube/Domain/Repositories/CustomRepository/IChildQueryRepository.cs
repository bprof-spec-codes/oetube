using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IChildQueryRepository<TEntity,TKey,TChildEntity,TChildQueryArgs>:IReadRepository<TEntity,TKey>
        where TEntity : class, IEntity<TKey>
        where TChildEntity : class, IEntity
        where TChildQueryArgs : IQueryArgs
    {
        public Task<PaginationResult<TChildEntity>> GetChildrenAsync(TEntity entity,
                                                          TChildQueryArgs? args = default,
                                                          bool includeDetails = false,
                                                          CancellationToken cancellationToken = default);
    }
    public interface IChildQueryAvaliableRepository<TEntity, TKey, TChildEntity, TChildQueryArgs>
        : IChildQueryRepository<TEntity, TKey, TChildEntity, TChildQueryArgs>
        where TEntity : class, IEntity<TKey>
        where TChildEntity : class, IEntity
        where TChildQueryArgs : IQueryArgs
    {
        public Task<PaginationResult<TChildEntity>> GetAvaliableChildrenAsync(Guid? requesterId,
                                                                              TEntity entity,
                                                                              TChildQueryArgs? args = default,
                                                                              bool includeDetails = false,
                                                                              CancellationToken cancellationToken = default);
    }
}