using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.Extensions;
using OeTube.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace OeTube.Domain.Services
{
    public class PlaylistItemManager : DomainService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistItemManager(IVideoRepository videoRepository, IPlaylistRepository playlistRepository)
        {
            _videoRepository = videoRepository;
            this._playlistRepository = playlistRepository;
        }

        public async Task<Playlist> UpdateItemsAsync(Playlist playlist, IEnumerable<Guid> items, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            items = items.Distinct();
            var videos = await _videoRepository.GetManyAsSetAsync(items, false, cancellationToken);
            if (videos.Count != items.Count())
            {
                throw new EntityNotFoundException(typeof(Video));
            }
            if (videos.Any(v => v.Id != playlist.CreatorId))
            {
                throw new InvalidOperationException("Video and playlist creators do not match.");
            }

            await _playlistRepository.UpdateItemsAsync(playlist, videos, autoSave, cancellationToken);
            return playlist;
        }
    }
}
