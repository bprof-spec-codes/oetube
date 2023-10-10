using OeTube.Domain.Entities.Groups;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;

namespace OeTube.Domain.Repositories.Queries
{
    public interface IUserGroupQuery
    {
        Task<IQueryable<OeTubeUser>> GetGroupMembersAsync(Group group);
        Task<IQueryable<Group>> GetJoinedGroupsAsync(OeTubeUser user);
        Task<IQueryable<OeTubeUser>> GetGroupDomainMembersAsync(Group group);
        Task<IQueryable<OeTubeUser>> GetGroupMembersWithoutDomainMembersAsync(Group group);
    }
}
