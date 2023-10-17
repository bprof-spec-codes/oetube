using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace OeTube.Domain.Repositories
{
    public interface IOeTubeUserRepository : IRepository<OeTubeUser, Guid>
    {
        Task<IQueryable<OeTubeUser>> GetByEmailDomainQueryableAsync(string emailDomain);
    }
}
