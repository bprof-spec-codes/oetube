using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.Extensions;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Repositories.Extensions
{
    public static class IncludeExtension
    {
        private static readonly Dictionary<Type, string[]> _includeStore
            = new Dictionary<Type, string[]>(PopulateIncludeStore());

        private static IEnumerable<KeyValuePair<Type, string[]>> PopulateIncludeStore()
        {
            yield return CreateInclude<Group>(nameof(Group.Members), nameof(Group.EmailDomains));
            yield return CreateInclude<Playlist>(nameof(Playlist.Items));
            yield return CreateInclude<Video>(nameof(Video.AccessGroups));
        }
        private static KeyValuePair<Type, string[]> CreateInclude<TEntity>(params string[] navigationProperties)
            where TEntity : class, IEntity
        {
            return new KeyValuePair<Type, string[]>(typeof(TEntity), navigationProperties);
        }

        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> queryable, bool includeDetails = true)
            where TEntity : class, IEntity
        {
            if (!includeDetails)
            {
                return queryable;
            }
            if (!_includeStore.TryGetValue(typeof(TEntity), out string[] value))
            {
                return queryable;
            }
            foreach (string item in value)
            {
                queryable = queryable.Include(item);
            }
            return queryable;
        }

    }
}
