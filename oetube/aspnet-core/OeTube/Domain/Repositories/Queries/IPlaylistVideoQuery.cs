using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.Application.Dtos;

namespace OeTube.Domain.Repositories.Queries
{
    public interface IPlaylistVideoQuery
    {
        Task<IQueryable<Video>> GetPlaylistVideosAsync(Playlist playlist);
        Task<IQueryable<VideoItem>> GetVideoItemsAsync();
    }
}
