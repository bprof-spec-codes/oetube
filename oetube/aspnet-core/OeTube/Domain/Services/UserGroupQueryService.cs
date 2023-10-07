using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using NUglify.JavaScript.Syntax;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using OeTube.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace OeTube.Domain.Services
{

    public interface IUserGroupQueryService
    {
        Task<IQueryable<Member>> GetAllMemberAsync();
        Task<IQueryable<Member>> GetDomainMembersAsync();
        Task<IQueryable<OeTubeUser>> GetGroupMembersAsync(Group group);
        Task<IQueryable<OeTubeUser>> GetGroupMembersByDomainAsync(Group group);
        Task<IQueryable<OeTubeUser>> GetGroupMembersWithDomainAsync(Group group);
        Task<IQueryable<Group>> GetJoinedGroupsAsync(OeTubeUser user);
    }

    public class UserGroupQueryService : DomainService, IUserGroupQueryService
    {
        private readonly IReadOnlyOeTubeUserRepository _users;
        private readonly IReadOnlyGroupRepository _groups;

        public UserGroupQueryService(IReadOnlyOeTubeUserRepository users, IReadOnlyGroupRepository groups)
        {
            _users = users;
            _groups = groups;
        }
        public async Task<IQueryable<Member>> GetDomainMembersAsync()
        {
            var domainMembers = from emailDomain in await _groups.GetEmailDomainsQueryableAsync()
                                join user in await _users.GetQueryableAsync()
                                on emailDomain.Domain equals user.EmailDomain
                                select new Member(emailDomain.GroupId, user.Id);
            return domainMembers;
        }

        public async Task<IQueryable<Member>> GetAllMemberAsync()
        {
            var domainMembers = await GetDomainMembersAsync();
            return domainMembers.Concat(await _groups.GetMembersQueryableAsync()).Distinct();
        }

        public async Task<IQueryable<Group>> GetJoinedGroupsAsync(OeTubeUser user)
        {
            var result = from @group in await _groups.GetQueryableAsync()
                         where @group.CreatorId != user.Id
                         join member in await GetAllMemberAsync()
                         on @group.Id equals member.GroupId
                         where member.UserId == user.Id
                         select @group;
            return result;
        }

        public async Task<IQueryable<OeTubeUser>> GetGroupMembersAsync(Group group)
        {
            var result = from member in await _groups.GetMembersQueryableAsync()
                         where member.GroupId == @group.Id
                         join user in await _users.GetQueryableAsync()
                         on member.UserId equals user.Id
                         select user;
            return result;
        }
         
        public async Task<IQueryable<OeTubeUser>> GetGroupMembersByDomainAsync(Group group)
        {
            var result = from emailDomain in await _groups.GetEmailDomainsQueryableAsync()
                         where emailDomain.GroupId == @group.Id
                         join user in await _users.GetQueryableAsync()
                         on emailDomain.Domain equals user.EmailDomain
                         where user.Id != @group.CreatorId
                         select user;
            return result;
        }

        public async Task<IQueryable<OeTubeUser>> GetGroupMembersWithDomainAsync(Group group)
        {
            var members = await GetGroupMembersAsync(group);
            var domain = await GetGroupMembersByDomainAsync(group);
            return domain.Concat(members).Distinct();
        }
    }
}
