using Microsoft.EntityFrameworkCore;
using OeTube.Data;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Domain.Repositories
{
    public interface IReadOnlyGroupRepository : IReadOnlyRepository<Group, Guid>
    {
        Task<IQueryable<EmailDomain>> GetEmailDomainsQueryableAsync();
        Task<IQueryable<Member>> GetMembersQueryableAsync();
    }
    public interface IGroupRepository: IReadOnlyGroupRepository,IRepository<Group,Guid>
    { }

    [ExposeServices(typeof(IReadOnlyGroupRepository),typeof(IGroupRepository))]
    public class GroupRepository 
        : EfCoreRepository<OeTubeDbContext, Group, Guid>,IGroupRepository, ITransientDependency
    {
        public GroupRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<IQueryable<EmailDomain>> GetEmailDomainsQueryableAsync()
        {
            return (await GetDbContextAsync()).Set<EmailDomain>().AsQueryable();
        }
        public async Task<IQueryable<Member>> GetMembersQueryableAsync()
        {
            return (await GetDbContextAsync()).Set<Member>().AsQueryable();
        }
        public override async Task<IQueryable<Group>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include(g => g.Members)
                                              .Include(g=>g.EmailDomains);
        }
    }
}
