using OeTube.Application.Caches.Composite;
using OeTube.Application.Caches.Source;
using OeTube.Application.Dtos.Groups;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace OeTube.Application.Caches
{
    public class GroupCacheService : CompositeCacheService<IGroupRepository,Group, Guid>, ITransientDependency
    {
 
        public GroupCacheService(IGroupRepository repository,IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser, ISourceCacheFactory sourceCacheFactory) : base(serviceProvider, currentUser, sourceCacheFactory,repository)
        {

            ConfigureMembersCount();
            ConfigureIsMember();
      
        }
        protected virtual void ConfigureIsMember()
        {
            RequesterDtoCache.ConfigureProperty<GroupDto, bool>(g => g.CurrentUserIsMember,
          async (key, group, userId) => await Repository.IsMemberAsync(userId, group!));
        }
        protected virtual void ConfigureMembersCount()
        {
            GlobalDtoCache.ConfigureProperty<GroupDto, int>(g => g.TotalMembersCount,
                   async (key, group, userId) => await Repository.GetMembersCountAsync(group!));
        }

        public async Task<bool> GetOrAddCurrentUserIsMemberAsync(Group group)
        {
            return await RequesterDtoCache.GetOrAddAsync<GroupDto, bool>(group.Id,group, g => g.CurrentUserIsMember);
        }
        public async Task<int> GetOrAddMembersCountAsync(Group group)
        {
            return await GlobalDtoCache.GetOrAddAsync<GroupDto, int>(group.Id,group, g => g.TotalMembersCount);
        }

    }
}