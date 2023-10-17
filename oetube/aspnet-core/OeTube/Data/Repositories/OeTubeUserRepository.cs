using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.Extensions;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Data.Repositories
{

    public class OeTubeUserRepository : EfCoreRepository<OeTubeDbContext, OeTubeUser,Guid>,IOeTubeUserRepository,ITransientDependency
    {
        public OeTubeUserRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<IQueryable<OeTubeUser>> GetByEmailDomainQueryableAsync(string emailDomain)
        {
            return (await GetQueryableAsync()).Where(u => u.EmailDomain == emailDomain);
        }

    }
}
