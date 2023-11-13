using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IParentUpdateRepository<TParentEntity, TChildEntity>
        where TParentEntity : class, IEntity
        where TChildEntity : class, IEntity
    {
        public Task<TParentEntity> UpdateChildrenAsync(TParentEntity entity,
                                                           IEnumerable<TChildEntity> childEntities,
                                                           bool autoSave = false,
                                                           CancellationToken cancellationToken = default);
    }

    public interface IParentUpdateRepositoryByKey<TParentEntity,TChildKey>
        where TParentEntity : class, IEntity
    {
        public Task<TParentEntity> UpdateChildrenAsync(TParentEntity entity,
                                                IEnumerable<TChildKey> childIds,
                                                bool autoSave = false,
                                                CancellationToken cancellationToken = default);

    }

}