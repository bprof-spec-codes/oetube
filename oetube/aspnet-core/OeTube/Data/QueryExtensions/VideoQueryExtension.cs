using IdentityModel.Client;
using JetBrains.Annotations;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using System.Linq.Expressions;

namespace OeTube.Data.QueryExtensions
{
    public static class VideoQueryExtension
    {
        public static IQueryable<Group> GetAccessGroups(this OeTubeDbContext context, Video video)
        {
            return from accessGroup in context.Set<AccessGroup>()
                   where accessGroup.VideoId == video.Id
                   join @group in context.Set<Group>()
                   on accessGroup.GroupId equals @group.Id
                   select @group;
        }
        public static IQueryable<Video> GetAvaliableVideos(this OeTubeDbContext context,Guid? requesterId)
        {
            var videos= context.Set<Video>();
            var publicAccess= from video in videos
                               where video.Access == AccessType.Public
                               select video;

            var creatorAccess = from video in videos
                                where video.CreatorId == requesterId
                                select video;


            var groupAccess = from video in videos
                              where video.Access == AccessType.Group
                              join accessGroup in context.Set<AccessGroup>()
                              on video.Id equals accessGroup.VideoId
                              join membership in context.GetMemberships()
                              on accessGroup.GroupId equals membership.Group!.Id
                              where membership.User!.Id == requesterId
                              select video;


            return publicAccess.Union(creatorAccess).Union(groupAccess).Where(v=>v.IsUploadCompleted).Distinct();
        }

        public static bool HasAccess(this OeTubeDbContext context, Guid? requesterId, Video video)
        {
            if (requesterId is null)
            {
                return video.Access == AccessType.Public;
            }
            else
            {
                
                if (video.Access == AccessType.Public || requesterId == video.CreatorId)
                {
                    return true;
                }
                if (video.Access == AccessType.Private)
                {
                    return false;
                }

                var groups = context.GetJoinedGroups(requesterId);

                var result = from accessGroups in context.Set<AccessGroup>()
                             where accessGroups.VideoId == video.Id
                             join @group in groups
                             on accessGroups.GroupId equals @group.Id
                             select groups;
                return result.Any();
            }
        }

        public static IQueryable<Video> GetUncompletedVideos(this OeTubeDbContext context, TimeSpan old = default)
        {
            var date = DateTime.Now - old;
            var result = from video in context.Set<Video>()
                         where !video.IsUploadCompleted && video.CreationTime <= date
                         select video;
            return result;
        }
    }
}