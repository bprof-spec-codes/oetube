using OeTube.Data;
using OeTube.Domain.Entities.Groups;
using OeTube.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Domain.Repositories
{
    public interface IReadOnlyOeTubeUserRepository : IReadOnlyRepository<OeTubeUser, Guid>
    {
        Task<IQueryable<OeTubeUser>> GetUsersByEmailDomainAsync(string emailDomain);
    }
    public interface IOeTubeUserRepository:IReadOnlyOeTubeUserRepository,IRepository<OeTubeUser,Guid>
    { }

    [ExposeServices(typeof(IReadOnlyOeTubeUserRepository),typeof(IOeTubeUserRepository))]
    public class OeTubeUserRepository 
        : EfCoreRepository<OeTubeDbContext, OeTubeUser, Guid>,IOeTubeUserRepository,ITransientDependency
    {
        public OeTubeUserRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
        public async Task<IQueryable<OeTubeUser>> GetUsersByEmailDomainAsync(string emailDomain)
        {
            return (await GetQueryableAsync()).Where(u => u.EmailDomain == emailDomain);
        }
    }
}
