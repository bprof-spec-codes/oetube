using OeTube.Data.Repositories.Groups;
using OeTube.Data.Repositories.Playlists;
using OeTube.Data.Repositories.Repos.GroupRepos;
using OeTube.Data.Repositories.Videos;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Entities;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace OeTube.Data.Repositories.Users
{
    public class UserRepository : CustomRepository<OeTubeUser, Guid, UserIncluder, UserFilter, IUserQueryArgs>, IUserRepository, ITransientDependency
    {
        private readonly GroupRepository _groupRepository;
        private readonly VideoRepository _videoRepository;
        private readonly VideoIncluder _videoIncluder;
        private readonly VideoFilter _videoFilter;
        private readonly PlaylistIncluder _playlistIncluder;
        private readonly PlaylistFilter _playlistFilter;
        private readonly GroupIncluder _groupIncluder;
        private readonly GroupFilter _groupFilter;

        public UserRepository(
            IDbContextProvider<OeTubeDbContext> dbContextProvider,
            UserIncluder includer,
            UserFilter filter,
            GroupRepository groupRepository,
            VideoRepository videoRepository,
            VideoIncluder videoIncluder,
            VideoFilter videoFilter,
            PlaylistIncluder playlistIncluder,
            PlaylistFilter playlistFilter,
            GroupIncluder groupIncluder,
            GroupFilter groupFilter) : base(dbContextProvider, includer, filter)
        {
            _groupRepository = groupRepository;
            _videoRepository = videoRepository;
            _videoIncluder = videoIncluder;
            _videoFilter = videoFilter;
            _playlistIncluder = playlistIncluder;
            _playlistFilter = playlistFilter;
            _groupIncluder = groupIncluder;
            _groupFilter = groupFilter;
        }

        public async Task<bool> HasAccess(OeTubeUser user, Video video)
        {
            if (video.Access == AccessType.Public || user.Id == video.CreatorId)
            {
                return true;
            }
            if (video.Access == AccessType.Private)
            {
                return false;
            }
            if (video.AccessGroups.Count == 0)
            {
                return false;
            }

            var groups = await _videoRepository.GetAccessGroupsAsync(video);
            var members = await _groupRepository.GetMembersWithDomainAsync();
            var result = (from @group in groups
                          join member in members
            on @group.Id equals member.GroupId
                          select @group.Id).FirstOrDefault();

            return result != default;
        }

        public async Task<IQueryable<Video>> GetAvaliableVideosAsync(OeTubeUser user)
        {
            var accessGroups = await _videoRepository.GetAccessGroupsAsync();
            var members = await _groupRepository.GetMembersWithDomainAsync();
            var videos = await GetQueryableAsync<Video>();

            var joined = (from member in members
                          where member.UserId == user.Id
                          join accessGroup in accessGroups
                          on member.GroupId equals accessGroup.GroupId
                          select accessGroup.VideoId).Distinct();

            var groupAccess = from video in videos
                              where video.Access == AccessType.Group
                              join id in joined
                              on video.Id equals id
                              select video;

            var trivialAccess = from video in videos
                                where video.Access == AccessType.Public || video.CreatorId == user.Id
                                select video;

            return groupAccess.Concat(trivialAccess).Distinct();
        }

        public async Task<IQueryable<Group>> GetJoinedGroupsAsync(OeTubeUser user)
        {
            var result = from @group in await GetQueryableAsync<Group>()
                         where @group.CreatorId != user.Id
                         join member in await GetQueryableAsync<Member>()
                         on @group.Id equals member.GroupId
                         where member.UserId == user.Id
                         select @group;
            return result;
        }

        private async Task<IQueryable<TEntity>> GetCreatedEntitiesAsync<TEntity>(OeTubeUser user)
            where TEntity : class, IEntity, IMayHaveCreator
        {
            var result = from entity in await GetQueryableAsync<TEntity>()
                         where entity.CreatorId != user.Id
                         select entity;
            return result;
        }

        public async Task<IQueryable<Group>> GetCreatedGroupsAsync(OeTubeUser user)
        {
            return await GetCreatedEntitiesAsync<Group>(user);
        }

        public async Task<IQueryable<Video>> GetCreatedVideosAsync(OeTubeUser user)
        {
            return await GetCreatedEntitiesAsync<Video>(user);
        }

        public async Task<IQueryable<Playlist>> GetCreatedPlaylistAsync(OeTubeUser user)
        {
            return await GetCreatedPlaylistAsync(user);
        }

        public async Task<List<Playlist>> GetCreatedPlaylistAsync(OeTubeUser user, IPlaylistQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetCreatedPlaylistAsync(user), _playlistIncluder, _playlistFilter, args, includeDetails, cancellationToken);
        }

        public async Task<List<Group>> GetCreatedGroupsAsync(OeTubeUser user, IGroupQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetCreatedGroupsAsync(user), _groupIncluder, _groupFilter, args, includeDetails, cancellationToken);
        }

        public async Task<List<Video>> GetCreatedVideosAsync(OeTubeUser user, IVideoQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetCreatedVideosAsync(user), _videoIncluder, _videoFilter, args, includeDetails, cancellationToken);
        }

        public async Task<List<Video>> GetAvaliableVideosAsync(OeTubeUser user, IVideoQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetAvaliableVideosAsync(user), _videoIncluder, _videoFilter, args, includeDetails, cancellationToken);
        }

        public async Task<List<Group>> GetJoinedGroupsAsync(OeTubeUser user, IGroupQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetJoinedGroupsAsync(user), _groupIncluder, _groupFilter, args, includeDetails, cancellationToken);
        }
    }
}