using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Entities.Groups;
using OeTube.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OeTube.Domain.Repositories;
using Volo.Abp.Identity;
using OeTube.Domain.Repositories.Extensions;

namespace OeTube.Data.Repositories
{
    public class GroupRepository :
        EfCoreRepository<OeTubeDbContext, Group, Guid>, IGroupRepository, ITransientDependency
    {
        public GroupRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        private async Task<DbSet<Member>> GetMembersAsync()
        {
            return (await GetDbContextAsync()).Set<Member>();
        }
        private async Task<DbSet<EmailDomain>> GetEmailDomainAsync()
        {
            return (await GetDbContextAsync()).Set<EmailDomain>();

        }
        public async Task<Group> UpdateMembersAsync(Group group,IEnumerable<IdentityUser> users,bool autoSave=false, CancellationToken cancellationToken=default)
        {
            var token=GetCancellationToken(cancellationToken);
            var members =await GetMembersAsync();
            var result=await members.Where(m => m.GroupId == group.Id).ToListAsync(token);
            members.RemoveRange(result);
            await members.AddRangeAsync(users.Select(u => new Member(group.Id, u.Id)),cancellationToken);
            if(autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
            return group;
        }

        public async Task<IQueryable<Member>> GetMembersQueryableAsync()
        {
            return await GetMembersAsync();
        }
        public async Task<IQueryable<EmailDomain>> GetEmailDomainsQueryableAsync()
        {
            return await GetEmailDomainAsync();
        }
        public override async Task<IQueryable<Group>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include();
        }

        public async Task<EntitySet<Group, Guid>> GetManyAsync(IEnumerable<Guid> keys, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var result = await this.GetManyQueryableAsync(keys, includeDetails);
            return new EntitySet<Group, Guid>(result);
        }
    }
}
