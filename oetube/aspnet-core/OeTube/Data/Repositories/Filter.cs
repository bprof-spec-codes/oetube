using OeTube.Domain.Repositories.QueryArgs;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;

namespace OeTube.Data.Repositories
{
    public interface IFilter<TEntity, TQueryArgs>
        where TEntity : class, IEntity
        where TQueryArgs : IQueryArgs
    {
        IQueryable<TEntity> FilterQueryable(IQueryable<TEntity> queryable, TQueryArgs args);
    }

    public abstract class Filter<TEntity, TQueryArgs> : IFilter<TEntity, TQueryArgs>
       where TEntity : class, IEntity
       where TQueryArgs : IQueryArgs
    {
        protected abstract Expression<Func<TEntity, bool>> GetFilter(TQueryArgs args);

        public IQueryable<TEntity> FilterQueryable(IQueryable<TEntity> queryable, TQueryArgs args)
        {
            if (args == null)
            {
                return queryable;
            }

            return queryable.Where(GetFilter(args));
        }
    }
}