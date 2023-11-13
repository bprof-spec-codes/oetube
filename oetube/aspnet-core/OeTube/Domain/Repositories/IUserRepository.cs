using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Domain.Repositories
{
    public interface IUserRepository :
        ICustomRepository<OeTubeUser, Guid, IUserQueryArgs>,
        IChildQueryRepository<OeTubeUser, Guid, Group, IGroupQueryArgs>
    {
    }
}