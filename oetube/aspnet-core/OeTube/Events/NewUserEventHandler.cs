using OeTube.Domain.Repositories;
using OeTube.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;

namespace OeTube.Events
{
    public class NewUserEventHandler : ILocalEventHandler<EntityCreatedEventData<IdentityUser>>,ITransientDependency
    {
        private readonly IOeTubeUserRepository _users;

        public NewUserEventHandler(IOeTubeUserRepository users)
        {
            _users = users;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
        {
            await _users.InsertAsync(new OeTubeUser(eventData.Entity),true);
        }
    }
}
