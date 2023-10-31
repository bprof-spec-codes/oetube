using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Entities;

namespace OeTube.Domain.Repositories
{
    public interface IQueryGroupRepository : IQueryRepository<Group, IGroupQueryArgs>
    {
        Task<List<Video>> GetAvaliableVideosAsync(Group group, IVideoQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<List<OeTubeUser>> GetGroupDomainMembersAsync(Group group, IUserQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<List<OeTubeUser>> GetGroupMembersAsync(Group group, IUserQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<List<OeTubeUser>> GetGroupMembersWithoutDomainMembersAsync(Group group, IUserQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);
    }

    public interface IUpdateGroupRepository : IUpdateRepository<Group, Guid>
    {
        Task<Group> UpdateMembersAsync(Group group, IEnumerable<Guid> userIds, bool autoSave = false, CancellationToken cancellationToken = default);
    }

    public interface IGroupRepository : ICustomRepository<Group, Guid, IGroupQueryArgs>, IQueryGroupRepository, IUpdateGroupRepository
    {
    }
}