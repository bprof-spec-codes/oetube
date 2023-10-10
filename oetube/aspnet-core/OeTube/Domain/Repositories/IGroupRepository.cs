using OeTube.Domain.Entities.Groups;
using OeTube.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace OeTube.Domain.Repositories
{
    public interface IGroupRepository : IRepository<Group, Guid>
    {
        Task<Group> UpdateMembersAsync(Group group, IEnumerable<IdentityUser> users, bool autoSave = false, CancellationToken cancellationToken = default);
    }
}
