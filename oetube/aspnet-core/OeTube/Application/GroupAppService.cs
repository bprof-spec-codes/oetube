using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OeTube.Application
{
    public class GroupAppService :
        ReadOnlyCustomAppService<IGroupRepository, Group, Guid, GroupDto, GroupListItemDto, IGroupQueryArgs, GroupQueryDto>,
        ICreateAppService<GroupDto, CreateUpdateGroupDto>,
        IUpdateAppService<GroupDto, Guid, CreateUpdateGroupDto>,
        IDeleteAppService<Guid>
    {
        public GroupAppService(IGroupRepository repository) : base(repository)
        {
        }

        public async Task<GroupDto> CreateAsync(CreateUpdateGroupDto input)
        {
            return await CreateAsync<IGroupRepository, Group, Guid, GroupDto, CreateUpdateGroupDto>(Repository, input);
        }

        public async Task DeleteAsync(Guid id)
        {
            await DeleteAsync<IGroupRepository, Group, Guid>(Repository, id);
        }

        public async Task<GroupDto> UpdateAsync(Guid id, CreateUpdateGroupDto input)
        {
            return await UpdateAsync<IGroupRepository, Group, Guid, GroupDto, CreateUpdateGroupDto>(Repository, id, input);
        }

        public async Task<PagedResultDto<UserListItemDto>> GetGroupMembersAsync(Guid id, UserQueryDto input)
        {
            var group = await Repository.GetAsync(id);
            return await GetListAsync<OeTubeUser, UserListItemDto>(() => Repository.GetGroupMembersAsync(group, input));
        }

        public async Task<GroupDto> UpdateMembersAsync(Guid id, ModifyMembersDto input)
        {
            var group = await Repository.GetAsync(id);
            return await UpdateAsync<Group, GroupDto>(async () => await Repository.UpdateMembersAsync(group, input.Members), group);
        }

        public async Task<GroupDto> UpdateEmailDomainsAsync(Guid id, ModifyEmailDomainsDto input)
        {
            var group = await Repository.GetAsync(id);
            return await UpdateAsync<Group, GroupDto>(async () =>
                        await Task.FromResult(group.UpdateEmailDomains(input.EmailDomains)), group);
        }
    }
}