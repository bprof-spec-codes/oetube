using Microsoft.EntityFrameworkCore;
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
        Task<IQueryable<OeTubeUser>> GetGroupMembersAsync(Group group);
        Task<IQueryable<OeTubeUser>> GetGroupMembersByDomainAsync(Group group);
        Task<IQueryable<OeTubeUser>> GetGroupMembersWithDomainAsync(Group group);
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
        public async Task<IQueryable<OeTubeUser>> GetGroupMembersAsync(Group group)
        {
            var members = (await _groups.GetMembersQueryableAsync())
                                    .Where(m=>m.GroupId==group.Id);
            var users = await _users.GetQueryableAsync();
            return users.Join(members, u => u.Id, m => m.UserId, (u, m) => u);
        }

        public async Task<IQueryable<OeTubeUser>> GetGroupMembersByDomainAsync(Group group)
        {
            var users = await _users.GetQueryableAsync();
            var emailDomains = (await _groups.GetEmailDomainsQueryableAsync())
                                 .Where(e=>e.GroupId==group.Id);
            return users.Join(emailDomains, u => u.EmailDomain, ed => ed.Domain, (u, ed) => u)
                        .Where(u=>u.Id!=group.CreatorId);
        }

        public async Task<IQueryable<OeTubeUser>> GetGroupMembersWithDomainAsync(Group group)
        {
            var members = await GetGroupMembersAsync(group);
            var domain = await GetGroupMembersByDomainAsync(group);
            return domain.Concat(members);
        }
    }
}
