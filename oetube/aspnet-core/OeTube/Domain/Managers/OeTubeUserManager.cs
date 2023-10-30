using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FileClasses;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Entities;
using OeTube.Infrastructure.FileClasses;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.EventBus.Local;

namespace OeTube.Domain.Managers
{
    public class OeTubeUserManager : DomainManager<IUserRepository, OeTubeUser, Guid, IUserQueryArgs,IUserFileClass>, IQueryUserRepository
    {
        public OeTubeUserManager(IUserRepository repository,
                           IFileContainerFactory containerFactory,
                           IBackgroundJobManager backgroundJobManager,
                           ILocalEventBus localEventBus) : base(repository, containerFactory, backgroundJobManager, localEventBus)
        {
        }

        public async Task UploadImage(Guid id, ByteContent content, CancellationToken cancellationToken = default)
        {
            await FileContainer.SaveFileAsync(new ImageFileClass(id), content, cancellationToken);
        }

        public Task<List<Video>> GetAvaliableVideosAsync(OeTubeUser user, IVideoQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetAvaliableVideosAsync(user, args, includeDetails, cancellationToken);
        }

        public Task<List<Group>> GetCreatedGroupsAsync(OeTubeUser user, IGroupQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetCreatedGroupsAsync(user, args, includeDetails, cancellationToken);
        }

        public Task<List<Playlist>> GetCreatedPlaylistAsync(OeTubeUser user, IPlaylistQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetCreatedPlaylistAsync(user, args, includeDetails, cancellationToken);
        }

        public Task<List<Video>> GetCreatedVideosAsync(OeTubeUser user, IVideoQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetCreatedVideosAsync(user, args, includeDetails, cancellationToken);
        }

        public Task<List<Group>> GetJoinedGroupsAsync(OeTubeUser user, IGroupQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetJoinedGroupsAsync(user, args, includeDetails, cancellationToken);
        }

        public Task<bool> HasAccess(OeTubeUser user, Video video)
        {
            return Repository.HasAccess(user, video);
        }
    }
}
