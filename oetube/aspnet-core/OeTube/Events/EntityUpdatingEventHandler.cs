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
    public class EntityUpdatingEventData<TEntity> 
        where TEntity:class,IEntity
    { 
        public TEntity Entity { get; }
        public EntityUpdatingEventData(TEntity entity)
        {
            Entity = entity;
        }
    }
    public abstract class EntityUpdatingEventHandler<TEntity, TKey, TCache> : ILocalEventHandler<EntityUpdatingEventData<TEntity>>
        where TEntity : class, IEntity<TKey>
        where TCache: ICompositeCacheService<TEntity,TKey>
    {
       protected TCache Cache { get; }

        protected EntityUpdatingEventHandler(TCache cache)
        {
            Cache = cache;
        }

        public async Task HandleEventAsync(EntityUpdatingEventData<TEntity> eventData)
        {
            await Cache.SourceCache.RefreshSourceAsync(eventData.Entity.Id);
        }
    }
    public class VideoUpdatedEventHandler : EntityUpdatingEventHandler<Video, Guid, VideoCacheService>,ITransientDependency
    {
        public VideoUpdatedEventHandler(VideoCacheService cache) : base(cache)
        {
        }
    }
    public class PlaylistUpdatedEventHandler : EntityUpdatingEventHandler<Playlist, Guid, PlaylistCacheService>,ITransientDependency
    {
        public PlaylistUpdatedEventHandler(PlaylistCacheService cache) : base(cache)
        {
        }
    }
    public class UserUpdatedEventHnadler : EntityUpdatingEventHandler<OeTubeUser, Guid, UserCacheService>,ITransientDependency
    {
        public UserUpdatedEventHnadler(UserCacheService cache) : base(cache)
        {
        }
    }
    public class GroupUpdatedEventHandler : EntityUpdatingEventHandler<Group, Guid, GroupCacheService>,ITransientDependency
    {
        public GroupUpdatedEventHandler(GroupCacheService cache) : base(cache)
        {
        }
    }
}