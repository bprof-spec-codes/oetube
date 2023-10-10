using OeTube.Data.Repositories;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.Queries;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;

namespace OeTube.Data.Queries
{

    public class CreatorQuery : ICreatorQuery
    {
        private readonly GroupRepository _groupRepository;
        private readonly VideoRepository _videoRepository;
        private readonly PlaylistRepository _playlistRepository;

        public CreatorQuery(GroupRepository groupRepository, VideoRepository videoRepository, PlaylistRepository playlistRepository)
        {
            _groupRepository = groupRepository;
            _videoRepository = videoRepository;
            _playlistRepository = playlistRepository;
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
            return await GetCreatedEntityAsync(user, _videoRepository);
        }
      

        public async Task<IQueryable<Playlist>> GetCreatedPlaylistsAsync(OeTubeUser user)
        {
            return await GetCreatedEntityAsync(user, _playlistRepository);
        }
       
        public async Task<IQueryable<Group>> GetCreatedGroupsAsync(OeTubeUser user)
        {
            return await GetCreatedEntityAsync(user, _groupRepository);
        }
    
    }
}
