using Microsoft.EntityFrameworkCore;
using OeTube.Data;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Domain.Repositories
{
    public interface IReadOnlyVideoRepository : IReadOnlyRepository<Video, Guid>
    {
        Task<IQueryable<AccessGroup>> GetAccessGroupsQueryableAsync();
    }
    public interface IVideoRepository:IReadOnlyVideoRepository,IRepository<Video,Guid>
    { }

    [ExposeServices(typeof(IReadOnlyVideoRepository),typeof(IVideoRepository))]
    public class VideoRepository : 
        EfCoreRepository<OeTubeDbContext, Video, Guid>,IVideoRepository,ITransientDependency
    {
        public VideoRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        { }
        public async Task<IQueryable<AccessGroup>> GetAccessGroupsQueryableAsync()
        {
            return (await GetDbContextAsync()).Set<AccessGroup>().AsQueryable();
        }
        public override async Task<IQueryable<Video>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include(v => v.AccessGroups);
        }
    }

}
