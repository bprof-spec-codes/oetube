using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace OeTube.Domain.Services
{
    public interface IGroupVideoQueryService
    {
        Task<IQueryable<Group>> GetAccessGroupsAsync(Video video);
        Task<IQueryable<Video>> GetAvaliableVideosAsync(Group group);
    }

    public class GroupVideoQueryService : DomainService, IGroupVideoQueryService
    {
        private readonly IReadOnlyGroupRepository _groups;
        private readonly IReadOnlyVideoRepository _videos;

        public GroupVideoQueryService(IReadOnlyGroupRepository groups, IReadOnlyVideoRepository videos)
        {
            _groups = groups;
            _videos = videos;
        }

        public async Task<IQueryable<Group>> GetAccessGroupsAsync(Video video)
        {
            var result = from accessGroup in await _videos.GetAccessGroupsQueryableAsync()
                         where accessGroup.VideoId == video.Id
                         join @group in await _groups.GetQueryableAsync()
                         on accessGroup.GroupId equals @group.Id
                         select @group;
            return result;
        }
        public async Task<IQueryable<Video>> GetAvaliableVideosAsync(Group group)
        {
            var result = from accessGroup in await _videos.GetAccessGroupsQueryableAsync()
                         where accessGroup.GroupId == @group.Id
                         join video in await _videos.GetQueryableAsync()
                         on accessGroup.VideoId equals video.Id
                         select video;
            return result;
        }
    }

}
