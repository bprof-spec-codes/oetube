using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Entities.Groups;
using OeTube.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OeTube.Domain.Repositories;
using Volo.Abp.Identity;
using OeTube.Domain.Repositories.Extensions;
using OeTube.Data.Repositories.Includers;

namespace OeTube.Data.Repositories
{
    public class GroupRepository :
        EfCoreRepository<OeTubeDbContext, Group, Guid>, IGroupRepository, ITransientDependency
    {
        private readonly IIncluder<Group> _includer;

        public GroupRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider, IIncluder<Group> includer) : base(dbContextProvider)
        {
            this._includer = includer;
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
            await members.AddRangeAsync(users.Select(u => new Member(group.Id, u.Id)), token);
            if(autoSave)
            {
                await SaveChangesAsync(token);
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
            return await _includer.IncludeAsync(GetQueryableAsync, true);
        }

       
    }
}
