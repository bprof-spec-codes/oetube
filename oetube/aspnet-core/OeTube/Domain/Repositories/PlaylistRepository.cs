using Microsoft.EntityFrameworkCore;
using OeTube.Data;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Domain.Repositories
{
    public interface IReadOnlyPlaylistRepository:IReadOnlyRepository<Playlist,Guid>
    {
        Task<IQueryable<VideoItem>> GetVideoItemsQueryableAsync();
    }
    public interface IPlaylistRepository:IReadOnlyPlaylistRepository,IRepository<Playlist,Guid>
    { }

    [ExposeServices(typeof(IReadOnlyPlaylistRepository), typeof(IPlaylistRepository))]
    public class PlaylistRepository 
        : EfCoreRepository<OeTubeDbContext, Playlist, Guid>, IPlaylistRepository,ITransientDependency
    {
        public PlaylistRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<IQueryable<VideoItem>> GetVideoItemsQueryableAsync()
        {
            return (await GetDbContextAsync()).Set<VideoItem>().AsQueryable();
        }

        public override async Task<IQueryable<Playlist>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include(p => p.Items);
        }
    }
    
}
