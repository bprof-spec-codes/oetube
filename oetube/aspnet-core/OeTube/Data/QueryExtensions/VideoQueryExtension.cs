using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;

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

        public static IQueryable<Video> GetAvaliableVideos(this OeTubeDbContext context, Guid? requesterId, IQueryable<Video>? videoQueryable = null)
        {
            videoQueryable ??= context.Set<Video>();
            videoQueryable = from video in videoQueryable
                             where video.IsUploadCompleted
                             select video;

            if (requesterId is null)
            {
                return from video in videoQueryable
                       where video.Access == AccessType.Public
                       select video;
            }

            var accessGroups = context.Set<AccessGroup>();
            var members = context.GetMembers();

            var joined = (from member in members
                          where member.UserId == requesterId
                          join accessGroup in accessGroups
                          on member.GroupId equals accessGroup.GroupId
                          select accessGroup.VideoId).Distinct();

            var groupAccess = from video in videoQueryable
                              where video.Access == AccessType.Group
                              join id in joined
                              on video.Id equals id
                              select video;

            var trivialAccess = from video in videoQueryable
                                where video.Access == AccessType.Public || video.CreatorId == requesterId
                                select video;

            return groupAccess.Concat(trivialAccess).Distinct();
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
                var groups = from accessGroup in context.Set<AccessGroup>()
                             where accessGroup.VideoId == video.Id
                             join @group in context.Set<Group>()
                             on accessGroup.GroupId equals @group.Id
                             select @group;

                var members = context.GetMembers();
                var result = (from @group in groups
                              join member in members
                on @group.Id equals member.GroupId
                              select @group.Id).FirstOrDefault();

                return result != default;
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