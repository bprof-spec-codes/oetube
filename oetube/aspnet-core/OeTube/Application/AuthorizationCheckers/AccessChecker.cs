using Microsoft.AspNetCore.Authorization;
using OeTube.Application.Caches;
using OeTube.Application.Caches.Access;
using OeTube.Application.Caches.Composite;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace OeTube.Application.AuthorizationCheckers
{
    public abstract class AccessChecker<TEntity, TKey, TAccessCacheService> : AuthorizationChecker, IAuthorizationManyChecker<TEntity>
        where TEntity : class, IEntity<TKey>
        where TAccessCacheService : ICompositeAccessCacheService<TEntity,TKey>
    {
        protected TAccessCacheService Cache { get; }

        protected AccessChecker(IAuthorizationService authorizationService, ICurrentUser currentUser, TAccessCacheService cache) : base(authorizationService, currentUser)
        {
            Cache = cache;
        }

        public override async Task CheckRightsAsync(object? requestedObject)
        {
            if (requestedObject is TEntity entity)
            {
                await Cache.AccessCacheService.GetOrAddAsync(entity);
            }
        }

        public async Task CheckRightsManyAsync(IEnumerable<TEntity> requestedObjects)
        {
            await Cache.AccessCacheService.SetManyAsync(requestedObjects, true);
        }
    }

    public class PlaylistAccessChecker : AccessChecker<Playlist, Guid, PlaylistCacheService>, IAuthorizationManyChecker<Playlist>, ITransientDependency
    {
        public PlaylistAccessChecker(IAuthorizationService authorizationService, ICurrentUser currentUser, PlaylistCacheService cache) : base(authorizationService, currentUser, cache)
        {
        }
    }

    public class VideoAccessChecker : AccessChecker<Video, Guid, VideoCacheService>, ITransientDependency
    {
        public VideoAccessChecker(IAuthorizationService authorizationService, ICurrentUser currentUser, VideoCacheService cache) : base(authorizationService, currentUser, cache)
        {
        }
    }
}