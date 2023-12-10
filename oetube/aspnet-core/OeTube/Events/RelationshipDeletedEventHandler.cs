using Microsoft.AspNetCore.Identity;
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
    public abstract class RelationshipDeletedEventHandler<TLeftEntity,TLeftKey,TLeftCache,TJunctionEntity,TRightEntity,TRightKey,TRightCache> : ILocalEventHandler<EntityDeletedEventData<TJunctionEntity>>
        where TLeftEntity : class, IEntity<TLeftKey>
        where TLeftCache:ICompositeCacheService<TLeftEntity,TLeftKey>
        where TJunctionEntity:class, IEntity
        where TRightEntity:class,IEntity<TRightKey>
        where TRightCache:ICompositeCacheService<TRightEntity,TRightKey>
    {
        public RelationshipDeletedEventHandler(TLeftCache leftCache,TRightCache rightCache)
        {
            LeftCache = leftCache;
            RightCache = rightCache;
        }

        protected TLeftCache LeftCache { get; }
        protected TRightCache RightCache { get; }
        protected abstract TLeftKey SelectLeftKey(TJunctionEntity entity);
        protected abstract TRightKey SelectRightKey(TJunctionEntity entity);
        public virtual async Task HandleEventAsync(EntityDeletedEventData<TJunctionEntity> eventData)
        {
           await LeftCache.SourceCache.RefreshSourceAsync(SelectLeftKey(eventData.Entity));
           await RightCache.SourceCache.RefreshSourceAsync(SelectRightKey(eventData.Entity));
        }
    }
    public class MemberDeletedEventHandler : RelationshipDeletedEventHandler<Group, Guid, GroupCacheService, Member, OeTubeUser, Guid, UserCacheService>,ITransientDependency
    {
        public MemberDeletedEventHandler(GroupCacheService leftCache, UserCacheService rightCache) : base(leftCache, rightCache)
        {
        }

        protected override Guid SelectLeftKey(Member entity)
        {
            return entity.GroupId;
        }

        protected override Guid SelectRightKey(Member entity)
        {
            return entity.UserId;
        }
    }
    public class AccessGroupDeletedEventHandler : RelationshipDeletedEventHandler<Video, Guid, VideoCacheService, AccessGroup, Group, Guid, GroupCacheService>,ITransientDependency
    {
        public AccessGroupDeletedEventHandler(VideoCacheService leftCache, GroupCacheService rightCache) : base(leftCache, rightCache)
        {
        }

        protected override Guid SelectLeftKey(AccessGroup entity)
        {
            return entity.VideoId;
        }

        protected override Guid SelectRightKey(AccessGroup entity)
        {
            return entity.GroupId;
        }
    }
    public class VideoItemDeletedEventHandler : RelationshipDeletedEventHandler<Playlist, Guid, PlaylistCacheService, VideoItem, Video, Guid, VideoCacheService>,ITransientDependency
    {
        public VideoItemDeletedEventHandler(PlaylistCacheService leftCache, VideoCacheService rightCache) : base(leftCache, rightCache)
        {
        }

        protected override Guid SelectLeftKey(VideoItem entity)
        {
            return entity.PlaylistId;
        }

        protected override Guid SelectRightKey(VideoItem entity)
        {
            return entity.VideoId;
        }
    }
}