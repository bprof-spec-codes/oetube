using Microsoft.EntityFrameworkCore;
using OeTube.Data.QueryExtensions;
using OeTube.Data.Repositories.Users;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
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

        public async Task<bool> IsMemberAsync(Guid? userId, Group group)
        {
            if (userId is null) return false;
            return (await GetDbContextAsync()).GetMembers(group)
                                              .OrderBy(u => u.Id)
                                              .FirstOrDefault(u => u.Id == userId) != null;
        }

        public async Task<int> GetMembersCountAsync(Group group, CancellationToken cancellationToken = default)
        {
            return await (await GetDbContextAsync()).GetMembers(group)
                                                     .CountAsync(cancellationToken);
        }

        public async Task<OeTubeUser?> GetCreatorAsync(Group entity, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await GetCreatorAsync<Group, OeTubeUser, UserIncluder>(entity, includeDetails, cancellationToken);
        }

        public async Task<Group> UpdateChildrenAsync(Group entity, IEnumerable<Guid> childIds, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var users = await GetQueryableAsync<IdentityUser>();
            var membersList = await users.Where(u => childIds.Contains(u.Id)).Select(u => u.Id).ToListAsync(cancellationToken);
            if (entity.CreatorId != null)
            {
                membersList.Remove(entity.CreatorId.Value);
            }

            var membersSet = await GetDbSetAsync<Member>();
            membersSet.RemoveRange(entity.Members);
            await membersSet.AddRangeAsync(membersList.Select(id => new Member(entity.Id, id)), cancellationToken);
            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
            return entity;
        }
        public async Task<PaginationResult<OeTubeUser>> GetMembersAsync(Group entity, IUserQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = (await GetDbContextAsync()).GetMembers(entity);
            return await CreateListAsync<OeTubeUser, UserIncluder, UserFilter, IUserQueryArgs>(
                queryable, args, includeDetails, cancellationToken);
        }
        public async Task<PaginationResult<OeTubeUser>> GetChildrenAsync(Group entity, IUserQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = (await GetDbContextAsync()).GetExplicitMembers(entity);
            return await CreateListAsync<OeTubeUser, UserIncluder, UserFilter, IUserQueryArgs>
                (queryable, args, includeDetails, cancellationToken);
        }
    }
}