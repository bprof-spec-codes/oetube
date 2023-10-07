using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace OeTube.Domain.Repositories
{
    public static class RepositoryExtension
    {
        public static async Task<IQueryable<TEntity>> GetManyAsync<TEntity, TKey>(this IReadOnlyRepository<TEntity, TKey> repository, IEnumerable<TKey> keys)
            where TEntity : Entity<TKey>
        {

            var entities = await repository.GetQueryableAsync();
            return entities.Join(keys, e => e.Id, k => k, (e, k) => e);
        }
        public static async Task<bool> KeysExistAsync<TEntity, TKey>(this IReadOnlyRepository<TEntity, TKey> repository, IEnumerable<TKey> keys)
            where TEntity : Entity<TKey>
        {
            var entities = await repository.GetManyAsync(keys);
            return entities.Count() == keys.Count();
        }
        public static async Task CheckKeysExistAsync<TEntity, TKey>(this IReadOnlyRepository<TEntity, TKey> repository, IEnumerable<TKey> keys)
         where TEntity : Entity<TKey>
        {
            if (!await repository.KeysExistAsync(keys))
            {
                throw new ArgumentException();
            }
        }
    }
}
