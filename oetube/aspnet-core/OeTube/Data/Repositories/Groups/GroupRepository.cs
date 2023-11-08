using Microsoft.EntityFrameworkCore;
using OeTube.Data.Repositories.Users;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;

namespace OeTube.Data.Repositories.Groups
{
    public class GroupRepository : 
        OeTubeRepository<Group, Guid, GroupIncluder, GroupFilter, IGroupQueryArgs>, 
        IGroupRepository,
        ITransientDependency
    {
        public GroupRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        protected async Task<IQueryable<OeTubeUser>> GetGroupMembersAsync(Group group)
        {
            var explicitMembers = from member in await GetQueryableAsync<Member>()
                                  where member.GroupId == @group.Id
                                  join user in await GetQueryableAsync<OeTubeUser>()
                                  on member.UserId equals user.Id
                                  select user;

            var domainMembers = from emailDomain in await GetQueryableAsync<EmailDomain>()
                                where emailDomain.GroupId == @group.Id
                                join user in await GetQueryableAsync<OeTubeUser>()
                                on emailDomain.Domain equals user.EmailDomain
                                where user.Id != @group.CreatorId
                                select user;

            return explicitMembers.Concat(domainMembers).Distinct();
        }
        public async Task<bool> IsMemberAsync(Guid? userId, Group group)
        {
            if (userId is null) return false;
            return (await GetGroupMembersAsync(group)).OrderBy(u => u.Id).FirstOrDefault(u => u.Id == userId) != null;
        }
        public async Task<int> GetMembersCountAsync(Group group,CancellationToken cancellationToken=default)
        {
            return  await (await GetGroupMembersAsync(group)).CountAsync(cancellationToken);
        }
        public async Task<OeTubeUser?> GetCreatorAsync(Group entity, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await GetCreatorAsync<Group, OeTubeUser, UserIncluder>(entity, includeDetails, cancellationToken);
        }

        public async Task<Group> UpdateChildEntitiesAsync(Group entity, IEnumerable<Guid> childIds, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var users =await GetQueryableAsync<IdentityUser>();
            var membersList = await users.Where(u=>childIds.Contains(u.Id)).Select(u=>u.Id).ToListAsync(cancellationToken);
            if (entity.CreatorId != null)
            {
                membersList.Remove(entity.CreatorId.Value);
            }

            var membersSet = await GetDbSetAsync<Member>();
            membersSet.RemoveRange(entity.Members);
            await membersSet.AddRangeAsync(membersList.Select(id => new Member(entity.Id,id)), cancellationToken);
            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
            return entity;
        }

        public async Task<PaginationResult<OeTubeUser>> GetChildEntitiesAsync(Group entity, IUserQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await CreateListAsync<OeTubeUser, UserIncluder, UserFilter, IUserQueryArgs>(
                await GetGroupMembersAsync(entity), args, includeDetails, cancellationToken);
        }
    }
}