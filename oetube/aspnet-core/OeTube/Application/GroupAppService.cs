using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using OeTube.Domain.Services;
using OeTube.Entities;
using System.Runtime.CompilerServices;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OeTube.Application
{
    public class GroupAppService : CrudAppService
                                                <Group, GroupDto,
                                                GroupItemDto, Guid,
                                                PagedAndSortedResultRequestDto,
                                                CreateUpdateGroupDto, CreateUpdateGroupDto>
    {
        private readonly IGroupRepository _groups;
        private readonly IUserGroupQueryService _userGroupQuery;
        private readonly IReadOnlyOeTubeUserRepository _users;
        public GroupAppService(IUserGroupQueryService userGroupQuery, IGroupRepository groups, IReadOnlyOeTubeUserRepository users) : base(groups)
        {
            _groups = groups;
            _userGroupQuery = userGroupQuery;
            _users = users;
        }
        private async Task<PagedResultDto<OeTubeUserItemDto>> GetUsersByGroupAsync
            (Guid id, Func<Group, Task<IQueryable<OeTubeUser>>> queryFunction, PagedAndSortedResultRequestDto input)
        {
            var group = await _groups.GetAsync(id);
            var users = await queryFunction(group);
            var result = users.PageBy(input)
                            .Select(ObjectMapper.Map<OeTubeUser, OeTubeUserItemDto>)
                            .ToList();
            return new PagedResultDto<OeTubeUserItemDto>(result.Count, result);
        }

        public async Task<PagedResultDto<OeTubeUserItemDto>> GetGroupMembersAsync(Guid id, PagedAndSortedResultRequestDto input)
        {
            return await GetUsersByGroupAsync(id, _userGroupQuery.GetGroupMembersWithDomainAsync, input);
        }
        public async Task AddMembersAsync(Guid id, ModifyMembersDto input)
        {

            var group = await _groups.GetAsync(id);
            CurrentUser.CheckIsOwner(group);
            await _users.CheckKeysExistAsync(input.Members);
            foreach (var item in input.Members)
            {
                group.AddMember(item);
            }
            await _groups.UpdateAsync(group, true);
        }
        public async Task RemoveMembersAsync(Guid id, ModifyMembersDto input)
        {
            var group = await _groups.GetAsync(id);
            CurrentUser.CheckIsOwner(group);
            foreach (var item in input.Members)
            {
                group.RemoveMember(item);
            }
            await _groups.UpdateAsync(group, true);
        }
        public async Task AddEmailDomainsAsync(Guid id, ModifyEmailDomainsDto input)
        {
            var group = await _groups.GetAsync(id);
            CurrentUser.CheckIsOwner(group);

            foreach (var item in input.EmailDomains)
            {
                group.AddEmailDomain(item);
            }
            await _groups.UpdateAsync(group, true);
        }
        public async Task RemoveEmailDomainsAsync(Guid id, ModifyEmailDomainsDto input)
        {
            var group = await _groups.GetAsync(id);
            CurrentUser.CheckIsOwner(group);

            foreach (var item in input.EmailDomains)
            {
                group.RemoveEmailDomain(item);
            }
            await _groups.UpdateAsync(group, true);
        }

    }
}
