using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface IHasAccessRepository<TEntity, TKey>:IReadRepository<TEntity,TKey>
        where TEntity:class,IEntity<TKey>
    {
        Task<bool> HasAccessAsync(Guid? requesterId, TEntity entity);
    }
    
}