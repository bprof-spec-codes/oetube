using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace OeTube.Domain.Repositories.Extensions
{
    public static class RepositoryExtension
    {
        public static async Task<IQueryable<TEntity>> GetManyQueryableAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository, IEnumerable<TKey> keys, bool includeDetails = false)
            where TEntity : class, IEntity<TKey>
        {
            var query = includeDetails ? await repository.WithDetailsAsync() : await repository.GetQueryableAsync();
            return query.Where(e => keys.Contains(e.Id));
        }

    }
}
