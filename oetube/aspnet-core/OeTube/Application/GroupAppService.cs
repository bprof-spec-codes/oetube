using Microsoft.AspNetCore.Mvc;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Infrastructure.FileClasses;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Managers;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Entities;
using OeTube.Infrastructure.FileClasses;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace OeTube.Application
{
    public class GroupAppService :
        ReadOnlyCustomAppService
        <GroupManager,IGroupRepository, Group, Guid, IGroupFileClass,
            GroupDto, GroupListItemDto, IGroupQueryArgs, GroupQueryDto>,
        ICreateAppService<GroupDto, CreateUpdateGroupDto>,
        IUpdateAppService<GroupDto, Guid, CreateUpdateGroupDto>,
        IDeleteAppService<Guid>
    {
        public GroupAppService(GroupManager manager) : base(manager)
        {
        }

        public async Task<GroupDto> CreateAsync(CreateUpdateGroupDto input)
        {
            return await CreateAsync<Group, Guid, GroupDto, CreateUpdateGroupDto>(Manager, input);
        }

        public async Task DeleteAsync(Guid id)
        {
            await DeleteAsync(Manager, id);
        }

        public async Task<GroupDto> UpdateAsync(Guid id, CreateUpdateGroupDto input)
        {
            return await UpdateAsync<Group, Guid, GroupDto, CreateUpdateGroupDto>(Manager, id, input);
        }

        [HttpGet("api/app/group/{id}/image")]
        public async Task<IRemoteStreamContent?> GetImageAsync(Guid id)
        {
            return await GetFileOrNullAsync(async () => await Manager.GetFileAsync(new ImageFileClass(id)));
        }

        public async Task UploadImageAsync(Guid id,IRemoteStreamContent input)
        {
            var group = await Manager.GetAsync(id);
            await UpdateAsync(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input);
                await Manager.UploadImage(id, content);
            }, group);
        }
        public async Task<PagedResultDto<UserListItemDto>> GetGroupMembersAsync(Guid id, UserQueryDto input)
        {
            var group = await Manager.GetAsync(id);
            return await GetListAsync<OeTubeUser, UserListItemDto>(() => Manager.GetGroupMembersAsync(group, input));
        }

        public async Task<GroupDto> UpdateMembersAsync(Guid id, ModifyMembersDto input)
        {
            var group = await Manager.GetAsync(id);
            return await UpdateAsync<Group, GroupDto>(async () => await Manager.UpdateMembersAsync(group, input.Members), group);
        }

        public async Task<GroupDto> UpdateEmailDomainsAsync(Guid id, ModifyEmailDomainsDto input)
        {
            var group = await Manager.GetAsync(id);
            return await UpdateAsync<Group, GroupDto>(async () =>
                        await Task.FromResult(group.UpdateEmailDomains(input.EmailDomains)), group);
        }
    }
}