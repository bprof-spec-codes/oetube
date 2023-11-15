using OeTube.Application.Caches;
using OeTube.Application.Caches.Composite;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace OeTube.Events
{
    public abstract class EntityUpdatedEventHandler<TEntity, TKey, TCache> : ILocalEventHandler<EntityUpdatedEventHandler<TEntity>>
        where TEntity : class, IEntity<TKey>
        where TCache: ICompositeCacheService<TEntity,TKey>
    {
       protected TCache Cache { get; }

        protected EntityUpdatedEventHandler(TCache cache)
        {
            Cache = cache;
        }

        public virtual async Task HandleEventAsync(EntityDeletedEventData<TEntity> eventData)
        {
            await Cache.SourceCache.RefreshSourceAsync(eventData.Entity.Id);
        }
    }
    public class VideoUpdatedEventHandler : EntityUpdatedEventHandler<Video, Guid, VideoCacheService>,ITransientDependency
    {
        public VideoUpdatedEventHandler(VideoCacheService cache) : base(cache)
        {
        }
    }
    public class PlaylistUpdatedEventHandler : EntityUpdatedEventHandler<Playlist, Guid, PlaylistCacheService>,ITransientDependency
    {
        public PlaylistUpdatedEventHandler(PlaylistCacheService cache) : base(cache)
        {
        }
    }
    public class UserUpdatedEventHnadler : EntityUpdatedEventHandler<OeTubeUser, Guid, UserCacheService>,ITransientDependency
    {
        public UserUpdatedEventHnadler(UserCacheService cache) : base(cache)
        {
        }
    }
    public class GroupUpdatedEventHandler : EntityUpdatedEventHandler<Group, Guid, GroupCacheService>,ITransientDependency
    {
        public GroupUpdatedEventHandler(GroupCacheService cache) : base(cache)
        {
        }
    }
}