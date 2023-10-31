using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Domain.Repositories
{
    public interface IQueryPlaylistRepository : IQueryRepository<Playlist, IPlaylistQueryArgs>
    {
        Task<List<Video>> GetPlaylistVideosAsync(Playlist playlist, IVideoQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);
    }

    public interface IUpdatePlaylistRepository : IUpdateRepository<Playlist, Guid>
    {
        Task<Playlist> UpdateItemsAsync(Playlist playlist, IEnumerable<Guid> videoIds, bool autoSave = false, CancellationToken cancellationToken = default);
    }

    public interface IPlaylistRepository : ICustomRepository<Playlist, Guid, IPlaylistQueryArgs>, IQueryPlaylistRepository, IUpdatePlaylistRepository
    {
    }
}