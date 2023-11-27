using OeTube.Application.Caches.Composite;
using OeTube.Application.Caches.Source;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Domain.Entities;
using OeTube.Domain.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace OeTube.Application.Caches
{
    public class UserCacheService : CompositeCacheService<IUserRepository,OeTubeUser, Guid>, ITransientDependency
    {
        public UserCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser, ISourceCacheFactory sourceCacheFactory, IUserRepository repository) : base(serviceProvider, currentUser, sourceCacheFactory, repository)
        {
            ConfigureCreatorName();
        }

        protected virtual void ConfigureCreatorName()
        {
            GlobalDtoCache.ConfigureProperty<CreatorDto, string>(c => c.Name, async (key, user, currentUserId) =>
            {
                user = await Repository.GetAsync(key);
                return user.Name;
            });
        }
        public async Task<string> GetOrAddCreatorNameAsync(Guid id)
        {
            return (await GlobalDtoCache.GetOrAddAsync<CreatorDto, string>(id, null, c => c.Name))!;
        }
    }
}