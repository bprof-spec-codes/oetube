using OeTube.Data.Repositories;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.Queries;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Queries
{
    public class PlaylistVideoQuery:IPlaylistVideoQuery,ITransientDependency
    {
        private readonly PlaylistRepository _playlistRepository;
        private readonly VideoRepository _videoRepository;

        public PlaylistVideoQuery(PlaylistRepository playlistRepository,
                                  VideoRepository videoRepository)
        {
            _playlistRepository = playlistRepository;
            _videoRepository = videoRepository;
        }
        public async Task<IQueryable<VideoItem>> GetVideoItemsAsync()
        {
            return await _playlistRepository.GetVideoItemsQueryableAsync();
        }
        public async Task<IQueryable<Video>> GetPlaylistVideosAsync(Playlist playlist)
        {
            var result = from videoItem in await _playlistRepository.GetVideoItemsQueryableAsync()
                         where videoItem.PlaylistId == playlist.Id
                         join video in await _videoRepository.GetQueryableAsync()
                         on videoItem.VideoId equals video.Id
                         orderby videoItem.Order
                         select video;
            return result;
        }
      
    }
}
