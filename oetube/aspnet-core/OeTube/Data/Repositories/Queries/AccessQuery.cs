using OeTube.Data.Repositories;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.Queries;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Queries
{
    public class AccessQuery : ITransientDependency, IAccessQuery
    {
        private readonly UserGroupQuery _userGroupQuery;
        private readonly GroupVideoQuery _groupVideoQuery;
        private readonly VideoRepository _videoRepository;

        public AccessQuery(UserGroupQuery userGroupQuery, GroupVideoQuery groupVideoQuery, VideoRepository videoRepository)
        {
            _userGroupQuery = userGroupQuery;
            _groupVideoQuery = groupVideoQuery;
            _videoRepository = videoRepository;
        }

        public async Task<bool> HasAccess(OeTubeUser user, Video video)
        {
            if (video.Access == AccessType.Public || user.Id == video.CreatorId)
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

            var groups = await _groupVideoQuery.GetAccessGroupsAsync(video);
            var members = await _userGroupQuery.GetMembersWithDomainAsync();
            var result = (from @group in groups
                          join member in members
                          on @group.Id equals member.GroupId
                          select @group.Id).FirstOrDefault();

            return result != default;
        }

   
        public async Task<IQueryable<Video>> GetAvaliableVideosAsync(OeTubeUser user)
        {
            var accessGroups = await _groupVideoQuery.GetAccessGroupsAsync();
            var members = await _userGroupQuery.GetMembersWithDomainAsync();
            var videos = await _videoRepository.GetQueryableAsync();

            var joined = (from member in members
                          where member.UserId == user.Id
                          join accessGroup in accessGroups
                          on member.GroupId equals accessGroup.GroupId
                          select accessGroup.VideoId).Distinct();

            var groupAccess = from video in videos
                              where video.Access == AccessType.Group
                              join id in joined
                              on video.Id equals id
                              select video;

            var trivialAccess = from video in videos
                                where video.Access == AccessType.Public || video.CreatorId == user.Id
                                select video;

            return groupAccess.Concat(trivialAccess).Distinct();
        }

    }
}
