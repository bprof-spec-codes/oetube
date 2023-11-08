using OeTube.Domain.Entities;
using OeTube.Domain.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;

namespace OeTube.Events
{
    public class NewUserEventHandler : ILocalEventHandler<EntityCreatedEventData<IdentityUser>>, ITransientDependency
    {
        private readonly IUserRepository _userRepository;

        public NewUserEventHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
        {
            await _userRepository.InsertAsync(new OeTubeUser(eventData.Entity), true);
        }
    }
}