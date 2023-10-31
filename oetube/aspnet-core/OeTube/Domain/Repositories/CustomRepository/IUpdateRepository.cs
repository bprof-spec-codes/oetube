using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IUpdateRepository<TEntity, TKey> : IReadRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        Task UpdateManyAsync(IEnumerable<TEntity> entity, bool autoSave = false, CancellationToken cancellationToken = default);
    }
}