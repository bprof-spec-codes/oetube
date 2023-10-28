using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Dtos.Videos;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OeTube.Application
{
    public class OeTubeUserAppService :
        ReadOnlyCustomAppService<IUserRepository, OeTubeUser, Guid, UserDto, UserListItemDto, IUserQueryArgs, UserQueryDto>,
        IUpdateAppService<UserDto, Guid, UpdateUserDto>
    {
        public OeTubeUserAppService(IUserRepository repository) : base(repository)
        {
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input)
        {
            return await UpdateAsync<IUserRepository, OeTubeUser, Guid, UserDto, UpdateUserDto>
                (Repository, id, input);
        }

        public async Task<PagedResultDto<GroupListItemDto>> GetJoinedGroupsAsync(Guid id, GroupQueryDto input)
        {
            var user = await Repository.GetAsync(id);
            return await GetListAsync<Group, GroupListItemDto>(async () => await Repository.GetJoinedGroupsAsync(user, input));
        }

        public async Task<PagedResultDto<GroupListItemDto>> GetCreatedGroupsAsync(Guid id, GroupQueryDto input)
        {
            var user = await Repository.GetAsync(id);
            return await GetListAsync<Group, GroupListItemDto>(async () => await Repository.GetCreatedGroupsAsync(user, input));
        }

        public async Task<PagedResultDto<VideoListItemDto>> GetCreatedVideosAsync(Guid id, VideoQueryDto input)
        {
            var user = await Repository.GetAsync(id);
            return await GetListAsync<Video, VideoListItemDto>(async () => await Repository.GetCreatedVideosAsync(user, input));
        }

        public async Task<PagedResultDto<VideoListItemDto>> GetAvaliableVideosAsync(Guid id, VideoQueryDto input)
        {
            var user = await Repository.GetAsync(id);
            return await GetListAsync<Video, VideoListItemDto>(async () => await Repository.GetAvaliableVideosAsync(user, input));
        }
    }
}