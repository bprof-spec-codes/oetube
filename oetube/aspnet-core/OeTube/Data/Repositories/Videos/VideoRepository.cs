using Microsoft.EntityFrameworkCore;
using OeTube.Data.QueryExtensions;
using OeTube.Data.Repositories.Groups;
using OeTube.Data.Repositories.Playlists;
using OeTube.Data.Repositories.Users;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Data.Repositories.Videos
{
    public class VideoRepository : OeTubeRepository
        <Video, Guid, VideoIncluder, VideoFilter, IVideoQueryArgs>, IVideoRepository, ITransientDependency
    {
        public VideoRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<OeTubeUser?> GetCreatorAsync(Video video, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await GetCreatorAsync<Video, OeTubeUser, UserIncluder>(video, includeDetails, cancellationToken);
        }

        public async Task<PaginationResult<Video>> GetAvaliableAsync(Guid? requesterId, IVideoQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = (await GetDbContextAsync()).GetAvaliableVideos(requesterId);
            return await CreateListAsync<Video, VideoIncluder, VideoFilter, IVideoQueryArgs>
                (queryable, args, includeDetails, cancellationToken);
        }

        public async Task<PaginationResult<Video>> GetUncompletedVideosAsync(TimeSpan old = default, IQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var result = (await GetDbContextAsync()).GetUncompletedVideos(old);
            return await CreateListAsync<Video, VideoIncluder>(result, args, includeDetails, cancellationToken);
        }

        public async Task<Video> UpdateChildrenAsync(Video entity, IEnumerable<Group> childEntities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var accessGroupSet = await GetDbSetAsync<AccessGroup>();

            accessGroupSet.RemoveRange(entity.AccessGroups);
            await accessGroupSet.AddRangeAsync(childEntities.Select(g => new AccessGroup(entity.Id, g.Id)), cancellationToken);
            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
            return entity;
        }

        public async Task<PaginationResult<Group>> GetChildrenAsync(Video entity, IGroupQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = (await GetDbContextAsync()).GetAccessGroups(entity);
            return await CreateListAsync<Group, GroupIncluder, GroupFilter, IGroupQueryArgs>
                (queryable, args, includeDetails, cancellationToken);
        }

        public async Task<bool> HasAccessAsync(Guid? requesterId, Video entity)
        {
            return (await GetDbContextAsync()).HasAccess(requesterId, entity);
        }
    }
}