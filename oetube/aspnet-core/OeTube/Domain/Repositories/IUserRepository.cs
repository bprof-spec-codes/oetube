using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Entities;

namespace OeTube.Domain.Repositories
{
    public interface IQueryUserRepository : IQueryRepository<OeTubeUser, IUserQueryArgs>
    {
        Task<List<Video>> GetAvaliableVideosAsync(OeTubeUser user, IVideoQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<List<Group>> GetCreatedGroupsAsync(OeTubeUser user, IGroupQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<List<Playlist>> GetCreatedPlaylistAsync(OeTubeUser user, IPlaylistQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<List<Video>> GetCreatedVideosAsync(OeTubeUser user, IVideoQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<List<Group>> GetJoinedGroupsAsync(OeTubeUser user, IGroupQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<bool> HasAccess(OeTubeUser user, Video video);
    }

    public interface IUserRepository : IQueryUserRepository, ICreateRepository<OeTubeUser, Guid>, IReadRepository<OeTubeUser, Guid>, IUpdateRepository<OeTubeUser, Guid>
    {
    }
}