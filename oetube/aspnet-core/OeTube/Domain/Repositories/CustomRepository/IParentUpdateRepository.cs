using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IParentUpdateRepository<TParentEntity, TChildEntity>
        where TParentEntity : class, IEntity
        where TChildEntity : class, IEntity
    {
        public Task<TParentEntity> UpdateChildEntitiesAsync(TParentEntity entity,
                                                           IEnumerable<TChildEntity> childEntities,
                                                           bool autoSave = false,
                                                           CancellationToken cancellationToken = default);
    }

    public interface IParentUpdateRepositoryByKey<TParentEntity,TChildKey>
        where TParentEntity : class, IEntity
    {
        public Task<TParentEntity> UpdateChildEntitiesAsync(TParentEntity entity,
                                                IEnumerable<TChildKey> childIds,
                                                bool autoSave = false,
                                                CancellationToken cancellationToken = default);

    }

}