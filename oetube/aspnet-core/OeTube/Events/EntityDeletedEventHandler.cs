using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FileContainers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace OeTube.Events
{
    public abstract class EntityDeletedEventHandler<TEntity,TKey> : ILocalEventHandler<EntityDeletedEventData<TEntity>>
        where TEntity:IEntity<TKey>
        where TKey:notnull
    {
        private readonly IFileContainer FileContainer;

        public EntityDeletedEventHandler(IFileContainerFactory factory)
        {
            FileContainer=factory.Create<TEntity>();
        }

        public async virtual Task HandleEventAsync(EntityDeletedEventData<TEntity> eventData)
        {
            await FileContainer.DeleteKeyFilesAsync(eventData.Entity.Id);
        }
    }
    public class VideoDeletedEventHandler : EntityDeletedEventHandler<Video, Guid>,ITransientDependency
    {
        public VideoDeletedEventHandler(IFileContainerFactory factory) : base(factory)
        {
        }
    }
    public class UserDeletedEventHandler : EntityDeletedEventHandler<OeTubeUser, Guid>, ITransientDependency
    {
        public UserDeletedEventHandler(IFileContainerFactory factory) : base(factory)
        {
        }
    }
    public class GroupDeletedEventHandler : EntityDeletedEventHandler<Group, Guid>, ITransientDependency
    {
        public GroupDeletedEventHandler(IFileContainerFactory factory) : base(factory)
        {
        }
    }
}