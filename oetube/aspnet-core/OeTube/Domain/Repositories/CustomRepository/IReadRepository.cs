using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IReadRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetManyAsync(IEnumerable<TKey> ids, bool includeDetails = false, CancellationToken cancellationToken = default);
    }
}