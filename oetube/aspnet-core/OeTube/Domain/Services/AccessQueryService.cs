using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Entities;
using Volo.Abp.Domain.Services;

namespace OeTube.Domain.Services
{
    public class AccessQueryService : DomainService
    {
        private readonly IUserGroupQueryService _userGroupQuery;
        private readonly IReadOnlyVideoRepository _videos;
        private readonly IGroupVideoQueryService _groupVideoQuery;
        private readonly IReadOnlyGroupRepository _groups;

        public AccessQueryService(IUserGroupQueryService userGroupQuery, IReadOnlyVideoRepository videos, IGroupVideoQueryService groupVideoQuery, IReadOnlyGroupRepository groups)
        {
            _userGroupQuery = userGroupQuery;
            _videos = videos;
            _groupVideoQuery = groupVideoQuery;
            _groups = groups;
        }

        public async Task<bool> HasAccess(OeTubeUser user,Video video)
        {
            if (video.Access==AccessType.Public||user.Id == video.CreatorId)
            {
                return true;
            }
            if(video.Access==AccessType.Private)
            {
                return false;
            }
            if (video.AccessGroups.Count == 0)
            {
                return false;
            }
            
            var groups =await _groupVideoQuery.GetAccessGroupsAsync(video);
            var members = await _userGroupQuery.GetAllMemberAsync();
            var result = (from @group in groups
                         join member in members
                         on @group.Id equals member.GroupId
                         select @group.Id).FirstOrDefault();

            return result != default;
        }

        public async Task<IQueryable<Video>> GetAvaliableVideosAsync(OeTubeUser user)
        {
            var accessGroups = await _videos.GetAccessGroupsQueryableAsync();
            var members = await _userGroupQuery.GetAllMemberAsync();

            var ids = (from member in members
                       where member.UserId==user.Id
                         join accessGroup in await _videos.GetAccessGroupsQueryableAsync()
                         on member.GroupId equals accessGroup.GroupId
                         select accessGroup.VideoId).Distinct();

            var groupVideos = from video in await _videos.GetQueryableAsync()
                         where video.Access == AccessType.Group
                         join id in ids
                         on video.Id equals id
                         select video;

            var publicVideos = from video in await _videos.GetQueryableAsync()
                               where video.Access == AccessType.Public
                               select video;


            return groupVideos.Concat(publicVideos);
        }
    }

}
