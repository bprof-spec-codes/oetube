using OeTube.Application.Caches.Source;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace OeTube.Application.Caches.Composite
{
    public class VideoCacheService : CompositeAccessCacheService<IVideoRepository, Video, Guid>, ITransientDependency
    {
        public VideoCacheService(IAbpLazyServiceProvider serviceProvider, ICurrentUser currentUser, ISourceCacheFactory sourceCacheFactory, IVideoRepository repository) : base(serviceProvider, currentUser, sourceCacheFactory, repository)
        {
        }
    }
}