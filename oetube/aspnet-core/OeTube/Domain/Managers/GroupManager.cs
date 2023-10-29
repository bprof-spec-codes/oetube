using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.Images;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Entities;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Content;
using Volo.Abp.EventBus.Local;

namespace OeTube.Domain.Managers
{
    public class GroupManager : DomainManager<IGroupRepository,Group,Guid,IGroupQueryArgs>,IGroupRepository
    {
        public GroupManager(IGroupRepository repository,
                            IFileContainerFactory containerFactory,
                            IBackgroundJobManager backgroundJobManager,
                            ILocalEventBus localEventBus) : base(repository, containerFactory, backgroundJobManager, localEventBus)
        {
        }

        public async Task UploadImage(Guid id, ByteContent content, CancellationToken cancellationToken=default)
        {
            await FileContainer.SaveAsync(new ImageFileClass(id), content,cancellationToken);
        }

        public Task<List<Video>> GetAvaliableVideosAsync(Group group, IVideoQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetAvaliableVideosAsync(group, args, includeDetails, cancellationToken);
        }

        public Task<List<OeTubeUser>> GetGroupDomainMembersAsync(Group group, IUserQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetGroupDomainMembersAsync(group, args, includeDetails, cancellationToken);
        }

        public Task<List<OeTubeUser>> GetGroupMembersAsync(Group group, IUserQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetGroupMembersAsync(group, args, includeDetails, cancellationToken);
        }

        public Task<List<OeTubeUser>> GetGroupMembersWithoutDomainMembersAsync(Group group, IUserQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetGroupMembersWithoutDomainMembersAsync(group, args, includeDetails, cancellationToken);
        }

        public Task<Group> InsertAsync(Group entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Repository.InsertAsync(entity, autoSave, cancellationToken);
        }

        public Task InsertManyAsync(IEnumerable<Group> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Repository.InsertManyAsync(entities, autoSave, cancellationToken);
        }

        public Task<Group> UpdateMembersAsync(Group group, IEnumerable<Guid> userIds, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Repository.UpdateMembersAsync(group, userIds, autoSave, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Repository.DeleteAsync(id, autoSave, cancellationToken);
        }

        public async Task DeleteAsync(Group entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Repository.DeleteAsync(entity, autoSave, cancellationToken);
        }

        public async Task DeleteManyAsync(IEnumerable<Guid> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Repository.DeleteManyAsync(ids, autoSave, cancellationToken);
        }

        public async Task DeleteManyAsync(IEnumerable<Group> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Repository.DeleteManyAsync(entities, autoSave, cancellationToken);
        }
    }
}
