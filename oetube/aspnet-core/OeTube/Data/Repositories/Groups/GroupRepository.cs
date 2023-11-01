using OeTube.Data.Repositories.Groups;
using OeTube.Data.Repositories.Users;
using OeTube.Data.Repositories.Videos;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;

namespace OeTube.Data.Repositories.Repos.GroupRepos
{
    public class GroupRepository : CustomRepository<Group, Guid, GroupIncluder, GroupFilter, IGroupQueryArgs>, IGroupRepository, ITransientDependency
    {
        private readonly VideoIncluder _videoIncluder;
        private readonly VideoFilter _videoFilter;
        private readonly UserIncluder _userIncluder;
        private readonly UserFilter _userFilter;

        public GroupRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider, GroupIncluder includer, GroupFilter filter, VideoIncluder videoIncluder, VideoFilter videoFilter, UserIncluder userIncluder, UserFilter userFilter) : base(dbContextProvider, includer, filter)
        {
            _videoIncluder = videoIncluder;
            _videoFilter = videoFilter;
            _userIncluder = userIncluder;
            _userFilter = userFilter;
        }

        public async Task<IQueryable<OeTubeUser>> GetGroupMembersWithoutDomainMembersAsync(Group group)
        {
            var result = from member in await GetMembersAsync()
                         where member.GroupId == @group.Id
                         join user in await GetQueryableAsync<OeTubeUser>()
                         on member.UserId equals user.Id
                         select user;
            return result;
        }

        public async Task<IQueryable<OeTubeUser>> GetGroupDomainMembersAsync(Group group)
        {
            var result = from emailDomain in await GetEmailDomainsAsync()
                         where emailDomain.GroupId == @group.Id
                         join user in await GetQueryableAsync<OeTubeUser>()
                         on emailDomain.Domain equals user.EmailDomain
                         where user.Id != @group.CreatorId
                         select user;
            return result;
        }

        public async Task<IQueryable<Video>> GetAvaliableVideosAsync(Group group)
        {
            var result = from accessGroup in await GetQueryableAsync<AccessGroup>()
                         where accessGroup.GroupId == @group.Id
                         join video in await GetQueryableAsync<Video>()
                         on accessGroup.VideoId equals video.Id
                         select video;
            return result;
        }

        public async Task<IQueryable<OeTubeUser>> GetGroupMembersAsync(Group group)
        {
            return (await GetGroupMembersWithoutDomainMembersAsync(group)).Concat(await GetGroupDomainMembersAsync(group)).Distinct();
        }

        public async Task<IQueryable<Member>> GetMembersAsync()
        {
            return await GetQueryableAsync<Member>();
        }

        public async Task<IQueryable<EmailDomain>> GetEmailDomainsAsync()
        {
            return await GetQueryableAsync<EmailDomain>();
        }

        public async Task<IQueryable<Member>> GetDomainMembersAsync()
        {
            var domainMembers = from emailDomain in await GetEmailDomainsAsync()
                                join user in await GetQueryableAsync<OeTubeUser>()
                                on emailDomain.Domain equals user.EmailDomain
                                select new Member(emailDomain.GroupId, user.Id);
            return domainMembers;
        }

        public async Task<IQueryable<Member>> GetMembersWithDomainAsync()
        {
            return (await GetMembersAsync()).Concat(await GetDomainMembersAsync()).Distinct();
        }

        public async Task<List<OeTubeUser>> GetGroupDomainMembersAsync(Group group, IUserQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetGroupDomainMembersAsync(group), _userIncluder, _userFilter, args, includeDetails, cancellationToken);
        }

        public async Task<List<OeTubeUser>> GetGroupMembersAsync(Group group, IUserQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetGroupMembersAsync(group), _userIncluder, _userFilter, args, includeDetails, cancellationToken);
        }

        public async Task<List<OeTubeUser>> GetGroupMembersWithoutDomainMembersAsync(Group group, IUserQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetGroupMembersWithoutDomainMembersAsync(group), _userIncluder, _userFilter, args, includeDetails, cancellationToken);
        }

        public async Task<List<Video>> GetAvaliableVideosAsync(Group group, IVideoQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetAvaliableVideosAsync(group), _videoIncluder, _videoFilter, args, includeDetails, cancellationToken);
        }

        public async Task<Group> UpdateMembersAsync(Group group, IEnumerable<Guid> userIds, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            userIds = userIds.Distinct();
            var userSet = (await GetDbContextAsync()).Set<IdentityUser>();
            var users = userSet.Where(u => userIds.Contains(u.Id));
            if (users.Count() != userIds.Count())
            {
                throw new EntityNotFoundException();
            }

            List<Guid> membersList = userIds.ToList();
            if (group.CreatorId != null)
            {
                membersList.Remove(group.CreatorId.Value);
            }

            var membersSet = (await GetDbContextAsync()).Set<Member>();
            membersSet.RemoveRange(group.Members);
            await membersSet.AddRangeAsync(membersList.Select(id => new Member(group.Id, id)), cancellationToken);
            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
            return group;
        }
    }
}