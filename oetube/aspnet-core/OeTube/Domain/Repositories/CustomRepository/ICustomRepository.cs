using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.CustomRepository
{
    public interface ICustomRepository<TEntity, TKey, TQueryArgs> :
        IInsertRepository<TEntity>,
        IUpdateRepository<TEntity, TKey>,
        IReadRepository<TEntity, TKey>,
        IQueryRepository<TEntity, TQueryArgs>,
        IDeleteRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TQueryArgs : IQueryArgs
    {
    }
}