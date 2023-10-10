using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Entities;
using Volo.Abp.Domain.Repositories;

namespace OeTube.Domain.Repositories
{
    public interface IPlaylistRepository : IRepository<Playlist, Guid>
    {
        Task<Playlist> UpdateItemsAsync(Playlist playlist, IEnumerable<Video> videos, bool autoSave = false, CancellationToken cancellationToken = default);
    }
}
