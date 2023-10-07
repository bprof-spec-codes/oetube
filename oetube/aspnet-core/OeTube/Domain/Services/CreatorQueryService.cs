using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Entities;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace OeTube.Domain.Services
{
    public interface ICreatorQueryService
    {
        Task<IQueryable<Group>> GetCreatedGroupsAsync(OeTubeUser user);
        Task<IQueryable<Playlist>> GetCreatedPlaylistsAsync(OeTubeUser user);
        Task<IQueryable<Video>> GetCreatedVideosAsync(OeTubeUser user);
    }

    public class CreatorQueryService : DomainService, ICreatorQueryService
    {
        private readonly IReadOnlyVideoRepository _videos;
        private readonly IReadOnlyPlaylistRepository _playlists;
        private readonly IReadOnlyGroupRepository _groups;

        public CreatorQueryService(IReadOnlyVideoRepository videos, IReadOnlyPlaylistRepository playlists, IReadOnlyGroupRepository groups)
        {
            _videos = videos;
            _playlists = playlists;
            _groups = groups;
        }
        private async Task<IQueryable<T>> GetCreatedEntityAsync<T>(OeTubeUser user, IReadOnlyRepository<T> repository)
            where T : class, IEntity, IMayHaveCreator
        {
            var result = from creation in await repository.GetQueryableAsync()
                         where creation.CreatorId == user.Id
                         select creation;
            return result;
        }
        public async Task<IQueryable<Video>> GetCreatedVideosAsync(OeTubeUser user)
        {
            return await GetCreatedEntityAsync(user, _videos);
        }
        public async Task<IQueryable<Playlist>> GetCreatedPlaylistsAsync(OeTubeUser user)
        {
            return await GetCreatedEntityAsync(user, _playlists);
        }
        public async Task<IQueryable<Group>> GetCreatedGroupsAsync(OeTubeUser user)
        {
            return await GetCreatedEntityAsync(user, _groups);
        }

    }
}
