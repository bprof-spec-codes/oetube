using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Domain.Repositories
{
    public interface IGroupRepository :
        ICustomRepository<Group, Guid, IGroupQueryArgs>,
        IMayHaveCreatorRepository<Group, Guid, OeTubeUser>,
        IChildQueryRepository<Group, Guid, OeTubeUser, IUserQueryArgs>,
        IParentUpdateRepositoryByKey<Group, Guid>
    {
        Task<PaginationResult<OeTubeUser>> GetMembersAsync(Group entity, IUserQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default);
        Task<int> GetMembersCountAsync(Group group, CancellationToken cancellationToken = default);

        Task<bool> IsMemberAsync(Guid? userId, Group group);
    }
}