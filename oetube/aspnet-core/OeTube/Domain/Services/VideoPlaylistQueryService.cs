using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace OeTube.Domain.Services
{
    public interface IVideoPlaylistQueryService
    {
        Task<IQueryable<Video>> GetPlaylistVideos(Playlist playlist);
    }
    public class VideoPlaylistQueryService : DomainService, IVideoPlaylistQueryService
    {
        private readonly IReadOnlyVideoRepository _videos;
        private readonly IReadOnlyPlaylistRepository _playlists;

        public VideoPlaylistQueryService(IReadOnlyVideoRepository videos, IReadOnlyPlaylistRepository playlists)
        {
            _videos = videos;
            _playlists = playlists;
        }

        public async Task<IQueryable<Video>> GetPlaylistVideos(Playlist playlist)
        {
            var result = from videoItem in await _playlists.GetVideoItemsQueryableAsync()
                         where videoItem.PlaylistId == playlist.Id
                         join video in await _videos.GetQueryableAsync()
                         on videoItem.VideoId equals video.Id
                         orderby videoItem.Order
                         select video;

            return result;
        }
    }
}
