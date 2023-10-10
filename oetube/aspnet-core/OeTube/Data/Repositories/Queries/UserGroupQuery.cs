using Nito.AsyncEx;
using OeTube.Data.Repositories;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.Queries;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace OeTube.Data.Queries
{
    public class UserGroupQuery : IUserGroupQuery, ITransientDependency
    {
        private readonly OeTubeUserRepository _userRepository;
        private readonly GroupRepository _groupRepository;
        public UserGroupQuery(OeTubeUserRepository userRepository,
            GroupRepository groupRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        public async Task<IQueryable<OeTubeUser>> GetGroupMembersWithoutDomainMembersAsync(Group group)
        {
            var result = from member in await _groupRepository.GetMembersQueryableAsync()
                         where member.GroupId == @group.Id
                         join user in await _userRepository.GetQueryableAsync()
                         on member.UserId equals user.Id
                         select user;
            return result;
        }
        public async Task<IQueryable<OeTubeUser>> GetGroupDomainMembersAsync(Group group)
        {
            var result = from emailDomain in await _groupRepository.GetEmailDomainsQueryableAsync()
                         where emailDomain.GroupId == @group.Id
                         join user in await _userRepository.GetQueryableAsync()
                         on emailDomain.Domain equals user.EmailDomain
                         where user.Id != @group.CreatorId
                         select user;
            return result;
        }

        public async Task<IQueryable<OeTubeUser>> GetGroupMembersAsync(Group group)
        {
            return (await GetGroupMembersWithoutDomainMembersAsync(group)).Concat(await GetGroupDomainMembersAsync(group)).Distinct();
        }
        public async Task<IQueryable<Group>> GetJoinedGroupsAsync(OeTubeUser user)
        {
            var result = from @group in await _groupRepository.GetQueryableAsync()
                         where @group.CreatorId != user.Id
                         join member in await GetMembersAsync()
                         on @group.Id equals member.GroupId
                         where member.UserId == user.Id
                         select @group;
            return result;
        }

        public async Task<IQueryable<Member>> GetMembersAsync()
        {
            return await _groupRepository.GetMembersQueryableAsync();
        }
        public async Task<IQueryable<Member>> GetDomainMembersAsync()
        {
            var domainMembers = from emailDomain in await _groupRepository.GetEmailDomainsQueryableAsync()
                                join user in await _userRepository.GetQueryableAsync()
                                on emailDomain.Domain equals user.EmailDomain
                                select new Member(emailDomain.GroupId, user.Id);
            return domainMembers;
        }
        public async Task<IQueryable<Member>> GetMembersWithDomainAsync()
        {
            return (await GetMembersAsync()).Concat(await GetDomainMembersAsync()).Distinct();
        }
      
    }
}
