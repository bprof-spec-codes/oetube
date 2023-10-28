using Microsoft.EntityFrameworkCore;
using OeTube.Data.Repositories.Groups;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using System.Linq.Dynamic.Core;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Data.Repositories.Videos
{
    public class VideoRepository : CustomRepository<Video, Guid, VideoIncluder, VideoFilter, IVideoQueryArgs>, IVideoRepository, ITransientDependency
    {
        private readonly GroupIncluder _groupIncluder;
        private readonly GroupFilter _groupFilter;

        public VideoRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider,
                               VideoIncluder includer,
                               VideoFilter filter,
                               GroupIncluder groupIncluder,
                               GroupFilter groupFilter) : base(dbContextProvider, includer, filter)
        {
            _groupFilter = groupFilter;
            _groupIncluder = groupIncluder;
        }

        public async Task<IQueryable<AccessGroup>> GetAccessGroupsAsync()
        {
            return await GetQueryableAsync<AccessGroup>();
        }

        public async Task<IQueryable<VideoResolution>> GetVideoResolutionsAsync()
        {
            return await GetQueryableAsync<VideoResolution>();
        }

        public async Task<IQueryable<Group>> GetAccessGroupsAsync(Video video)
        {
            var result = from accessGroup in await GetAccessGroupsAsync()
                         where accessGroup.VideoId == video.Id
                         join @group in await GetQueryableAsync<Group>()
                         on accessGroup.GroupId equals @group.Id
                         select @group;
            return result;
        }

        public async Task<IQueryable<Video>> GetUncompletedVideosAsync(TimeSpan? old = null)
        {
            var result = from video in await GetQueryableAsync()
                         where !video.IsUploadCompleted && video.CreationTime <= DateTime.Now - old
                         select video;
            return result;
        }

        public async Task<List<Video>> GetUncompletedVideosAsync(TimeSpan? old = null, IVideoQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetUncompletedVideosAsync(old), _includer, _filter, args, includeDetails, cancellationToken);
        }

        public async Task<List<Group>> GetAccessGroupsAsync(Video video, IGroupQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetAccessGroupsAsync(video), _groupIncluder, _groupFilter, args, includeDetails, cancellationToken);
        }

        public async Task<Video> UpdateAccessGroupsAsync(Video video, IEnumerable<Guid> groupIds, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            groupIds = groupIds.Distinct();

            var groupSet = (await GetDbContextAsync()).Set<Group>();
            var groups = groupSet.Where(g => groupIds.Contains(g.Id));
            if (groups.Count() != groupIds.Count())
            {
                throw new EntityNotFoundException();
            }

            var accessGroupSet = (await GetDbContextAsync()).Set<AccessGroup>();

            accessGroupSet.RemoveRange(video.AccessGroups);
            await accessGroupSet.AddRangeAsync(groups.Select(g => new AccessGroup(video.Id, g.Id)), cancellationToken);
            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
            return video;
        }
    }
}