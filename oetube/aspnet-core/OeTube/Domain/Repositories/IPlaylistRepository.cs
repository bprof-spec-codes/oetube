using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Domain.Repositories
{
    public interface IPlaylistRepository :
        ICustomRepository<Playlist, Guid, IPlaylistQueryArgs>,
        IMayHaveCreatorRepository<Playlist, Guid, OeTubeUser>,
        IParentUpdateRepository<Playlist,Video>,
        IParentReadRepository<Playlist,Video,IVideoQueryArgs>,
        IHasAccessibilityRepository<Playlist, Guid, IPlaylistQueryArgs>
    {

    }
}