using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Extensions;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.Queries;
using OeTube.Domain.Services;
using OeTube.Entities;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace OeTube.Application
{

    [Authorize]
    public class GroupAppService : CreatorCrudAppService
                                                <Group, GroupDto,
                                                GroupItemDto, Guid,
                                                PagedAndSortedResultRequestDto,
                                                CreateUpdateGroupDto, CreateUpdateGroupDto>
    {
        private readonly IGroupRepository _groups;
        private readonly IUserGroupQuery _userGroupQuery;
        private readonly GroupMemberManager _groupMemberManager;

        public GroupAppService(IGroupRepository groups, IUserGroupQuery userGroupQuery, GroupMemberManager groupMemberManager)
        :base(groups)
        {
            _groups = groups;
            _userGroupQuery = userGroupQuery;
            _groupMemberManager = groupMemberManager;
        }

        public async Task<PagedResultDto<OeTubeUserItemDto>> GetGroupMembersAsync(Guid id, PagedAndSortedResultRequestDto input)
        {
            var group = await GetEntityByIdAsync(id);
            var result = await _userGroupQuery.GetGroupMembersAsync(group);

            return await result.ToPagedResultDtoAsync<OeTubeUser, OeTubeUserItemDto>(ObjectMapper,input);
        }
        
        public async Task UpdateMembersAsync(Guid id, ModifyMembersDto input)
        {
            var group = await GetEntityByIdWithCheckOwnerAndUpdatePolicyAsync(id);
            await _groupMemberManager.UpdateMembersAsync(group, input.Members,true);
        }
        public async Task UpdateEmailDomainsAsync(Guid id, ModifyEmailDomainsDto input)
        {
            var group = await GetEntityByIdWithCheckOwnerAndUpdatePolicyAsync(id);
            group.UpdateEmailDomains(input.EmailDomains);
            await _groups.UpdateAsync(group);

        }
       
    }
}
