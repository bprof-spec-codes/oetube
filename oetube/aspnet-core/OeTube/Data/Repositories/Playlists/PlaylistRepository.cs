using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using OeTube.Data.QueryExtensions;
using OeTube.Data.Repositories.Users;
using OeTube.Data.Repositories.Videos;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Data.Repositories.Playlists
{
    public class PlaylistRepository :
        OeTubeRepository<Playlist, Guid, PlaylistIncluder, PlaylistFilter, IPlaylistQueryArgs>,
        IPlaylistRepository,
        ITransientDependency
    {
        public PlaylistRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        private TimeSpan GetTotalDuration(IQueryable<Video> videos)
        {
            if (videos.Any())
            {
                var totalSeconds = videos.Select(v=>v.Duration).ToList().Sum(d => d.TotalSeconds);
                return TimeSpan.FromSeconds(totalSeconds);
            }
            else
            {
                return TimeSpan.Zero;
            }
        }

        public async Task<TimeSpan> GetAvaliableTotalDurationAsync(Guid? requesterId, Playlist playlist)
        {
            return GetTotalDuration((await GetDbContextAsync()).GetAvaliableVideos(requesterId, playlist));
        }

        public async Task<TimeSpan> GetTotalDurationAsync(Playlist playlist)
        {
            return GetTotalDuration((await GetDbContextAsync()).GetVideos(playlist));
        }
        public async Task<PaginationResult<Playlist>> GetAvaliableAsync(Guid? requesterId, IPlaylistQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = (await GetDbContextAsync()).GetAvaliablePlaylists(requesterId);
            return await CreateListAsync<Playlist, PlaylistIncluder, PlaylistFilter, IPlaylistQueryArgs>
                (queryable, args, includeDetails, cancellationToken);
        }

        public async Task<PaginationResult<Video>> GetAvaliableChildrenAsync(Guid? requesterId, Playlist entity, IVideoQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = (await GetDbContextAsync()).GetAvaliableVideos(requesterId, entity);
            return await CreateListAsync<Video, VideoIncluder, VideoFilter, IVideoQueryArgs>
                (queryable, args, includeDetails, cancellationToken);
        }

        public async Task<PaginationResult<Video>> GetChildrenAsync(Playlist entity, IVideoQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = (await GetDbContextAsync()).GetVideos(entity);
            return await CreateListAsync<Video, VideoIncluder, VideoFilter, IVideoQueryArgs>
                (queryable, args, includeDetails, cancellationToken);
        }

        public async Task<OeTubeUser?> GetCreatorAsync(Playlist entity, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await GetCreatorAsync<Playlist, OeTubeUser, UserIncluder>(entity, includeDetails, cancellationToken);
        }

        public async Task<bool> HasAccessAsync(Guid? requesterId, Playlist entity)
        {
            return (await GetDbContextAsync()).HasAccess(requesterId, entity);
        }
      
        public async Task<Playlist> UpdateChildrenAsync(Playlist entity, IEnumerable<Video> childEntities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var videoItemsSet = await GetDbSetAsync<VideoItem>();


            childEntities = childEntities.Where(e => e.CreatorId == entity.CreatorId);
            videoItemsSet.RemoveRange(entity.Items);
            var items = childEntities.Select((i, idx) => new VideoItem(entity.Id, idx, i.Id)).ToList();
            await videoItemsSet.AddRangeAsync(items, cancellationToken);

            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
            return entity;
        }
        public async Task<int> GetAvaliableItemsCountAsync(Guid? requesterId,Playlist playlist,CancellationToken cancellationToken = default)
        {
            return await (await GetDbContextAsync()).GetAvaliableVideos(requesterId, playlist).CountAsync(cancellationToken);
        }
        public async Task<int> GetItemsCountAsync(Playlist playlist,CancellationToken cancellationToken=default)
        {
            return await (await GetDbSetAsync<VideoItem>()).Where(i => i.PlaylistId == playlist.Id).CountAsync(cancellationToken);
        }
    }
}