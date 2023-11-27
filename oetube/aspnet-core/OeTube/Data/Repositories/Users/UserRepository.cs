using OeTube.Data.QueryExtensions;
using OeTube.Data.Repositories.Groups;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Data.Repositories.Users
{
    public class UserRepository :
        OeTubeRepository<OeTubeUser, Guid, UserIncluder, UserFilter, IUserQueryArgs>,
        IUserRepository,
        ITransientDependency
    {
        public UserRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PaginationResult<Group>> GetChildrenAsync(OeTubeUser entity, IGroupQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = (await GetDbContextAsync()).GetJoinedGroups(entity.Id);
            return await CreateListAsync<Group, GroupIncluder, GroupFilter, IGroupQueryArgs>(queryable, args, includeDetails, cancellationToken);
        }
    }
}