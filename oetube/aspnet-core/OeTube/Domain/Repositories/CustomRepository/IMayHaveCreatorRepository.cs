using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IMayHaveCreatorRepository<TEntity, TKey, TCreator>
        where TEntity : class, IEntity<TKey>, IMayHaveCreator
        where TCreator : class, IEntity<Guid>
    {
        public Task<TCreator?> GetCreatorAsync(TEntity entity, bool includeDetails = false, CancellationToken cancellationToken = default);
    }
}