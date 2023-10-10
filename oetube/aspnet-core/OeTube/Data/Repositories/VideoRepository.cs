using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.Extensions;
using System.Linq.Expressions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Data.Repositories
{
    public class VideoRepository : EfCoreRepository<OeTubeDbContext, Video, Guid>, IVideoRepository, ITransientDependency
    {
        public VideoRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        private async Task<DbSet<AccessGroup>> GetAccessGroupsAsync()
        {
            return (await GetDbContextAsync()).Set<AccessGroup>();
        }
        public async Task<IQueryable<AccessGroup>> GetAccessGroupsQueryableAsync()
        {
            return await GetAccessGroupsAsync();
        }
        public async Task<Video> UpdateAccessGroupsAsync(Video video, IEnumerable<Group> groups, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var token = GetCancellationToken(cancellationToken);
            var accessGroups = await GetAccessGroupsAsync();
            var result = await accessGroups.Where(ag => ag.VideoId == video.Id).ToListAsync(token);
            accessGroups.RemoveRange(result);
            await accessGroups.AddRangeAsync(groups.Select(g => new AccessGroup(video.Id, g.Id)));
            if (autoSave)
            {
                await SaveChangesAsync(token);
            }
            return video;
        }
        public override async Task<IQueryable<Video>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include();
        }
    }
}
