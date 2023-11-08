using OeTube.Data.Repositories.Users;
using OeTube.Data.Repositories.Videos;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
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

        public async Task<PaginationResult<Playlist>> GetAvaliableAsync(Guid? requesterId, IPlaylistQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await CreateListAsync<Playlist, PlaylistIncluder, PlaylistFilter, IPlaylistQueryArgs>
                (await GetAvaliablePlaylistsAsync(requesterId), args, includeDetails, cancellationToken);
        }

        public async Task<PaginationResult<Video>> GetChildEntitiesAsync(Playlist entity, IVideoQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await CreateListAsync<Video, VideoIncluder, VideoFilter, IVideoQueryArgs>
                (await GetChildEntitiesAsync<Playlist, Guid, VideoItem, Video, Guid>(entity), args, includeDetails, cancellationToken);
        }

        public async Task<OeTubeUser?> GetCreatorAsync(Playlist entity, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await GetCreatorAsync<Playlist, OeTubeUser, UserIncluder>(entity, includeDetails, cancellationToken);
        }

        public async Task<bool> HasAccessAsync(Guid? requesterId, Playlist entity)
        {
            return await HasPlaylistAccessAsync(requesterId, entity);
        }

        public async Task<Playlist> UpdateChildEntitiesAsync(Playlist entity, IEnumerable<Video> childEntities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var videoItemsSet = await GetDbSetAsync<VideoItem>();
            childEntities = childEntities.Where(e => e.CreatorId == entity.CreatorId);
            videoItemsSet.RemoveRange(entity.Items);
            await videoItemsSet.AddRangeAsync(childEntities.Select((i, idx) => new VideoItem(entity.Id, idx, i.Id)), cancellationToken);

            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
            return entity;
        }
    }
}