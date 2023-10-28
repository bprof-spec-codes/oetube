using OeTube.Data.Repositories.Videos;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Data.Repositories.Playlists
{
    public class PlaylistRepository : CustomRepository<Playlist, Guid, PlaylistIncluder, PlaylistFilter, IPlaylistQueryArgs>, IPlaylistRepository, ITransientDependency
    {
        private readonly VideoIncluder _videoIncluder;
        private readonly VideoFilter _videoFilter;

        public PlaylistRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider, PlaylistIncluder includer, PlaylistFilter filter, VideoIncluder videoIncluder, VideoFilter videoFilter) : base(dbContextProvider, includer, filter)
        {
            _videoIncluder = videoIncluder;
            _videoFilter = videoFilter;
        }

        public async Task<IQueryable<Video>> GetPlaylistVideosAsync(Playlist playlist)
        {
            var result = from videoItem in await GetQueryableAsync<VideoItem>()
                         where videoItem.PlaylistId == playlist.Id
                         join video in await GetQueryableAsync<Video>()
                         on videoItem.VideoId equals video.Id
                         orderby videoItem.Order
                         select video;
            return result;
        }

        public async Task<List<Video>> GetPlaylistVideosAsync(Playlist playlist, IVideoQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetPlaylistVideosAsync(playlist), _videoIncluder, _videoFilter, args, includeDetails, cancellationToken);
        }

        public async Task<Playlist> UpdateItemsAsync(Playlist playlist, IEnumerable<Guid> videoIds, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            videoIds = videoIds.Distinct();
            var videoSet = (await GetDbContextAsync()).Set<Video>();
            var videos = videoSet.Where(v => videoIds.Contains(v.Id));

            if (videos.Any(v => v.Id != playlist.CreatorId))
            {
                throw new InvalidOperationException("Video and playlist creators do not match.");
            }
            var videoItemsSet = (await GetDbContextAsync()).Set<VideoItem>();

            videoItemsSet.RemoveRange(playlist.Items);
            await videoItemsSet.AddRangeAsync(videos.Select((i, idx) => new VideoItem(playlist.Id, idx, i.Id)), cancellationToken);

            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
            return playlist;
        }
    }
}