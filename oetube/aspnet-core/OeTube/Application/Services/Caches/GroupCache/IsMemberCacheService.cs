using OeTube.Domain.Repositories;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using OeTube.Domain.Entities.Groups;

namespace OeTube.Application.Services.Caches.GroupCache
{
    public interface IIsMemberCacheService
    {
        Task<bool> IsMemberAsync(Guid? userId, Group group, CancellationToken cancellationToken = default);
    }

    public class IsMemberCacheService : ITransientDependency, IIsMemberCacheService
    {
        private readonly IDistributedCache<IsMemberCacheItem, IsMemberCacheKey> _cache;
        private readonly IGroupRepository _groupRepository;
        public IsMemberCacheService(IDistributedCache<IsMemberCacheItem, IsMemberCacheKey> cache, IGroupRepository groupRepository)
        {
            _cache = cache;
            _groupRepository = groupRepository;
        }
        private DistributedCacheEntryOptions GetOptions()
        {
            return new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
            };
        }
        public async Task<bool> IsMemberAsync(Guid? userId, Group group, CancellationToken cancellationToken = default)
        {
            async Task<IsMemberCacheItem> Factory()
            {
                return new IsMemberCacheItem()
                {
                    IsMember = await _groupRepository.IsMemberAsync(userId, group)
                };
            }
            if (userId is null)
            {
                return false;
            }
            var key = new IsMemberCacheKey()
            {
                UserId = userId.Value,
                GroupId = group.Id
            };
            var item = await _cache.GetOrAddAsync(key, Factory, GetOptions, false, true, cancellationToken);
            return item!.IsMember;
        }
    }
}
