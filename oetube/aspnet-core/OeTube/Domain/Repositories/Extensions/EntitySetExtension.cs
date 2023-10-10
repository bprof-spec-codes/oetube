using Microsoft.EntityFrameworkCore;
using OeTube.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace OeTube.Domain.Repositories.Extensions
{
    public static class EntitySetExtension
    {
        public static async Task<EntitySet<TEntity, TKey>> GetManyAsSetAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository, IEnumerable<TKey> keys, bool includeDetails = false, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity<TKey>, IHasAtomicKey<TKey>
         where TKey : notnull
        {
            var result = await repository.GetManyQueryableAsync(keys, includeDetails);
            return await result.ToEntitySetAsync<TEntity, TKey>(cancellationToken);
        }
        public static async Task<EntitySet<TEntity, TKey>> ToEntitySetAsync<TEntity, TKey>(this IQueryable<TEntity> queryable, CancellationToken cancellationToken = default)
         where TEntity : class, IEntity<TKey>, IHasAtomicKey<TKey>
         where TKey : notnull
        {

            var set = new EntitySet<TEntity, TKey>();
            await foreach (var element in queryable.AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                set.Add(element);
            }

            return set;
        }
    }

}
