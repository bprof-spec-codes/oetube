using OeTube.Data.Repositories.Groups;
using OeTube.Data.Repositories.Playlists;
using OeTube.Data.Repositories.Videos;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Data.Repositories.Users
{
    public class UserRepository : 
        OeTubeRepository<OeTubeUser, Guid, UserIncluder, UserFilter, IUserQueryArgs>, 
        IUserRepository,
        ITransientDependency
    {
        public UserRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PaginationResult<Video>> GetAvaliableCreatedEntititesAsync(Guid creatorId, Guid? requesterId, IVideoQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var videos = await GetCreatedEntitiesAsync<Video>(creatorId);
            var result = await GetAvaliableVideosAsync(requesterId, videos);
            return await CreateListAsync<Video, VideoIncluder, VideoFilter, IVideoQueryArgs>(result, args, includeDetails, cancellationToken);
        }
        public async Task<PaginationResult<Playlist>> GetAvaliableCreatedEntititesAsync(Guid creatorId, Guid? requesterId, IPlaylistQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var playlists = await GetCreatedEntitiesAsync<Playlist>(creatorId);
            var result = await GetAvaliablePlaylistsAsync(requesterId, playlists);
            return await CreateListAsync<Playlist, PlaylistIncluder, PlaylistFilter, IPlaylistQueryArgs>(result, args, includeDetails, cancellationToken);
        }

        public async Task<PaginationResult<Group>> GetCreatedEntitiesAsync(Guid creatorId, IGroupQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await CreateListAsync<Group, GroupIncluder, GroupFilter, IGroupQueryArgs>
                (await GetCreatedEntitiesAsync<Group>(creatorId), args, includeDetails, cancellationToken);
        }

        public async Task<PaginationResult<Video>> GetCreatedEntitiesAsync(Guid creatorId, IVideoQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await CreateListAsync<Video, VideoIncluder, VideoFilter, IVideoQueryArgs>
                (await GetCreatedEntitiesAsync<Video>(creatorId), args, includeDetails, cancellationToken);
        }

        public async Task<PaginationResult<Playlist>> GetCreatedEntitiesAsync(Guid creatorId, IPlaylistQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await CreateListAsync<Playlist, PlaylistIncluder, PlaylistFilter, IPlaylistQueryArgs>
                    (await GetCreatedEntitiesAsync<Playlist>(creatorId), args, includeDetails, cancellationToken);
        }

        public async Task<PaginationResult<Group>> GetJoinedGroupsAsync(Guid userId, IGroupQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var result = from @group in await GetQueryableAsync<Group>()
                         where @group.CreatorId != userId
                         join member in await GetMembersAsync()
                         on @group.Id equals member.GroupId
                         where member.UserId == userId
                         select @group;

            return await CreateListAsync<Group, GroupIncluder, GroupFilter, IGroupQueryArgs>(result, args, includeDetails, cancellationToken);
        }

    }
}