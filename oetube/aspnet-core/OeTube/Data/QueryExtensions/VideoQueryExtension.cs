using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;

namespace OeTube.Data.QueryExtensions
{
    public class VideoAccessibility
    {
        public OeTubeUser? User { get; set; }
        public Video? Video { get; set; }
    }

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
        public static IQueryable<VideoAccessibility> GetVideoAccessibilities(this OeTubeDbContext context, IQueryable<Video>? videos=null)
        {
            videos ??= context.Set<Video>();

            var publicAccess = from video in videos
                               where video.Access == AccessType.Public
                               from user in context.Set<OeTubeUser>()
                               select new VideoAccessibility()
                               {
                                   Video = video,
                                   User = user
                               };

            var privateAccess = from video in videos
                                where video.Access == AccessType.Private
                                join user in context.Set<OeTubeUser>()
                                on video.CreatorId equals user.Id
                                select new VideoAccessibility()
                                {
                                    Video = video,
                                    User = user
                                };

            var groupAccess = from video in videos
                              where video.Access == AccessType.Group
                              join accessGroup in context.Set<AccessGroup>()
                              on video.Id equals accessGroup.VideoId
                              join membership in context.GetMemberships()
                              on accessGroup.GroupId equals membership.Group!.Id
                              select new VideoAccessibility()
                              {
                                  Video = video,
                                  User = membership.User
                              };

            var result = publicAccess.Union(privateAccess).Union(groupAccess).Distinct()
                       .Where(a => a.Video!.IsUploadCompleted);
            return result;
        }

        public static IQueryable<Video> GetAvaliableVideos(this OeTubeDbContext context,Guid? requesterId,IQueryable<Video>? videos=null)
        {
            return from accessibility in GetVideoAccessibilities(context,videos)
                   where accessibility.User!.Id == requesterId
                   select accessibility.Video;
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
                if (video.AccessGroups.Count == 0)
                {
                    return false;
                }

                var groups = context.GetJoinedGroups(requesterId);

                return groups.Any(g => video.AccessGroups.Contains(g.Id));
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