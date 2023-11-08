using OeTube.Domain.Repositories;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using OeTube.Domain.Entities.Groups;

namespace OeTube.Application.Services.Caches.GroupCache
{
    public interface IGroupMembersCountCacheService
    {
        Task<int> GetTotalCountAsync(Group group, CancellationToken cancellationToken = default);
    }

    public class GroupMembersCountCacheService : ITransientDependency, IGroupMembersCountCacheService
    {
        private readonly IDistributedCache<GroupMembersCountCacheItem, GroupMembersCountCacheKey> _cache;
        private readonly IGroupRepository _groupRepository;
        public GroupMembersCountCacheService(IDistributedCache<GroupMembersCountCacheItem, GroupMembersCountCacheKey> cache, IGroupRepository groupRepository)
        {
            _cache = cache;
            _groupRepository = groupRepository;
        }
        private DistributedCacheEntryOptions GetOptions()
        {
            return new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(3)
            };
        }
        public async Task<int> GetTotalCountAsync(Group group, CancellationToken cancellationToken = default)
        {
            async Task<GroupMembersCountCacheItem> Factory()
            {
                return new GroupMembersCountCacheItem()
                {
                    TotalMembersCount = await _groupRepository.GetMembersCountAsync(group, cancellationToken)
                };
            }
            var key = new GroupMembersCountCacheKey()
            {
                GroupId = group.Id
            };
            var item = await _cache.GetOrAddAsync(key, Factory, GetOptions, false, true, cancellationToken);
            return item!.TotalMembersCount;
        }

    }
}
