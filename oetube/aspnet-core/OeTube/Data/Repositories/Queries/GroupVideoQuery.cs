using OeTube.Data.Repositories;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Queries
{

    public class GroupVideoQuery : IGroupVideoQuery, ITransientDependency
    {
        private readonly GroupRepository _groupRepository;
        private readonly VideoRepository _videoRepository;

        public GroupVideoQuery(GroupRepository groupRepository,
                               VideoRepository videoRepository)
        {
            _groupRepository = groupRepository;
            _videoRepository = videoRepository;
        }
        public async Task<IQueryable<AccessGroup>> GetAccessGroupsAsync()
        {
            return await _videoRepository.GetAccessGroupsQueryableAsync();
        }
        public async Task<IQueryable<Group>> GetAccessGroupsAsync(Video video)
        {
            var result = from accessGroup in await _videoRepository.GetAccessGroupsQueryableAsync()
                         where accessGroup.VideoId == video.Id
                         join @group in await _groupRepository.GetQueryableAsync()
                         on accessGroup.GroupId equals @group.Id
                         select @group;
            return result;
        }


        public async Task<IQueryable<Video>> GetAvaliableVideosAsync(Group group)
        {
            var result = from accessGroup in await _videoRepository.GetAccessGroupsQueryableAsync()
                         where accessGroup.GroupId == @group.Id
                         join video in await _videoRepository.GetQueryableAsync()
                         on accessGroup.VideoId equals video.Id
                         select video;
            return result;
        }

     
    }
}
