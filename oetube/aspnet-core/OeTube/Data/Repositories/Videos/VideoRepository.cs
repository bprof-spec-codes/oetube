using OeTube.Data.Repositories.Groups;
using OeTube.Data.Repositories.Users;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Data.Repositories.Videos
{

    public class VideoRepository : OeTubeRepository
        <Video, Guid, VideoIncluder, VideoFilter, IVideoQueryArgs>, IVideoRepository,ITransientDependency
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
            return await CreateListAsync<Video, VideoIncluder, VideoFilter, IVideoQueryArgs>
                (await GetAvaliableVideosAsync(requesterId), args, includeDetails, cancellationToken);
        }

        public async Task<PaginationResult<Video>> GetUncompletedVideosAsync(TimeSpan? old = null,IQueryArgs? args=null,bool includeDetails=false, CancellationToken cancellationToken=default)
        {
            var date = DateTime.Now - old;
            var result = from video in await GetQueryableAsync<Video>()
                         where !video.IsUploadCompleted && video.CreationTime <= date
                         select video;
            return await CreateListAsync<Video, VideoIncluder>(result, args, includeDetails, cancellationToken);
        }
        public async Task<Video> UpdateChildEntitiesAsync(Video entity, IEnumerable<Group> childEntities, bool autoSave = false, CancellationToken cancellationToken = default)
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
     
        public async Task<PaginationResult<Group>> GetChildEntitiesAsync(Video entity, IGroupQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await CreateListAsync<Group, GroupIncluder, GroupFilter, IGroupQueryArgs>
               (await GetChildEntitiesAsync<Video, Guid, AccessGroup, Group, Guid>(entity), args, includeDetails, cancellationToken);
        }

        public Task<bool> HasAccessAsync(Guid? requesterId, Video entity)
        {
            return HasVideoAccessAsync(requesterId, entity);
        }
    }
}