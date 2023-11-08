using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;

namespace OeTube.Data.Repositories
{
    public interface IIncluder<T>
     where T : class, IEntity
    {
        IQueryable<T> Include(IQueryable<T> queryable);

    }

    public abstract class Includer<T> : IIncluder<T>
        where T : class, IEntity
    {
        protected abstract IEnumerable<string> GetNavigationProperties();

        public IQueryable<T> Include(IQueryable<T> queryable)
        {
            foreach (string item in GetNavigationProperties())
            {
                queryable = queryable.Include(item);
            }
            return queryable;
        }
    }

}