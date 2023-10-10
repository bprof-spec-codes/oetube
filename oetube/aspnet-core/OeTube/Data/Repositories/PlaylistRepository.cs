using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.Extensions;
using OeTube.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Data.Repositories
{

    public class PlaylistRepository :
        EfCoreRepository<OeTubeDbContext, Playlist, Guid>, IPlaylistRepository, ITransientDependency
    {
        public PlaylistRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        private async Task<DbSet<VideoItem>> GetVideoItemsAsync()
        {
            return (await GetDbContextAsync()).Set<VideoItem>();
        }
        public async Task<IQueryable<VideoItem>> GetVideoItemsQueryableAsync()
        {
            return await GetVideoItemsAsync();
        }
        public async Task<Playlist> UpdateItemsAsync(Playlist playlist, IEnumerable<Video> videos, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var token = GetCancellationToken(cancellationToken);
            var videoItems = await GetVideoItemsAsync();
            var result = await videoItems.Where(vi => vi.PlaylistId == playlist.Id).ToListAsync(token);
            videoItems.RemoveRange(result);
            int i = 0;
            foreach (var item in videos)
            {
                await videoItems.AddAsync(new VideoItem(playlist.Id, i, item.Id),token);
                i++;
            }

            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
            return playlist;
        }
        public override async Task<IQueryable<Playlist>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include();
        }

        public async Task<List<Playlist>> GetListManyAsync(IEnumerable<Guid> keys, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var result = await this.GetManyQueryableAsync(keys, includeDetails);
            return await result.ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
