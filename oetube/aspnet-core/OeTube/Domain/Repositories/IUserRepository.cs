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
        ICreatorRepository<OeTubeUser, Group, IGroupQueryArgs>,
        ICreatorRepository<OeTubeUser, Video, IVideoQueryArgs>,
        ICreatorRepository<OeTubeUser, Playlist, IPlaylistQueryArgs>,
        IHasAccessiblityCreatorRepository<Video, IVideoQueryArgs>,
        IHasAccessiblityCreatorRepository<Playlist, IPlaylistQueryArgs>
    {
        Task<PaginationResult<Group>> GetJoinedGroupsAsync(Guid userId, IGroupQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default);
    }
}