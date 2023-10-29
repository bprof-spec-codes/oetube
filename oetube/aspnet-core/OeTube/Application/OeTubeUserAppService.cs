using Microsoft.AspNetCore.Mvc;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Dtos.Videos;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.Images;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Managers;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace OeTube.Application
{
    public class OeTubeUserAppService :
        ReadOnlyCustomAppService<OeTubeUserManager, OeTubeUser, Guid, UserDto, UserListItemDto, IUserQueryArgs, UserQueryDto>,
        IUpdateAppService<UserDto, Guid, UpdateUserDto>
    {
        public OeTubeUserAppService(OeTubeUserManager manager) : base(manager)
        {
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input)
        {
            return await UpdateAsync<OeTubeUserManager, OeTubeUser, Guid, UserDto, UpdateUserDto>
                (Manager, id, input);
        }

        [HttpGet("api/app/ou-tube-user/{id}/image")]
        public async Task<IRemoteStreamContent?> GetImageAsync(Guid id)
        {
            return await GetFileOrNullAsync(async () => await Manager.GetFileAsync(new ImageFileClass(id)));
        }

        public async Task UploadImageAsync(Guid id, IRemoteStreamContent input)
        {
            var group = await Manager.GetAsync(id);
            await UpdateAsync(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input);
                await Manager.UploadImage(id, content);
            }, group);
        }


        public async Task<PagedResultDto<GroupListItemDto>> GetJoinedGroupsAsync(Guid id, GroupQueryDto input)
        {
            var user = await Manager.GetAsync(id);
            return await GetListAsync<Group, GroupListItemDto>(async () => await Manager.GetJoinedGroupsAsync(user, input));
        }

        public async Task<PagedResultDto<GroupListItemDto>> GetCreatedGroupsAsync(Guid id, GroupQueryDto input)
        {
            var user = await Manager.GetAsync(id);
            return await GetListAsync<Group, GroupListItemDto>(async () => await Manager.GetCreatedGroupsAsync(user, input));
        }

        public async Task<PagedResultDto<VideoListItemDto>> GetCreatedVideosAsync(Guid id, VideoQueryDto input)
        {
            var user = await Manager.GetAsync(id);
            return await GetListAsync<Video, VideoListItemDto>(async () => await Manager.GetCreatedVideosAsync(user, input));
        }

        public async Task<PagedResultDto<VideoListItemDto>> GetAvaliableVideosAsync(Guid id, VideoQueryDto input)
        {
            var user = await Manager.GetAsync(id);
            return await GetListAsync<Video, VideoListItemDto>(async () => await Manager.GetAvaliableVideosAsync(user, input));
        }
    }
}