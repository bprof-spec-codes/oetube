using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Repositories.Extensions;
using System.Linq;
using Volo.Abp.Domain.Entities;

namespace OeTube.Data.Repositories.Includers
{
    public interface IIncluder<T>
     where T : class, IEntity
    {
        IQueryable<T> Include(IQueryable<T> queryable, bool includeDetails);
        Task<IQueryable<T>> IncludeAsync(Func<Task<IQueryable<T>>> awaitableQueryable, bool includeDetails);
    }
    public abstract class Includer<T> : IIncluder<T>
        where T : class, IEntity
    {
        protected abstract IEnumerable<string> GetNavigationProperties();
        public IQueryable<T> Include(IQueryable<T> queryable, bool includeDetails)
        {
            if (!includeDetails)
            {
                return queryable;
            }


            foreach (string item in GetNavigationProperties())
            {
                queryable = queryable.Include(item);
            }
            return queryable;
        }
        public async Task<IQueryable<T>> IncludeAsync(Func<Task<IQueryable<T>>> awaitableQueryable,bool includeDetails)
        {
            var queryable = await awaitableQueryable();
            return Include(queryable, includeDetails);
        }
    }

}
