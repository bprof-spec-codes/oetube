using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IInsertRepository<TEntity> 
        where TEntity : class, IEntity
    {
        Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
    }
}