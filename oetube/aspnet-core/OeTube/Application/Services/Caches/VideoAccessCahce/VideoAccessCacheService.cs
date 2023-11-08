using OeTube.Domain.Repositories;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Entities;
using Volo.Abp.VirtualFileSystem;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Volo.Abp.Users;
using Volo.Abp.Domain.Repositories;

namespace OeTube.Application.Services.Caches.VideoAccess
{
    public interface IVideoAccessCacheService
    {
        Task CheckAccessAsync(Guid? requesterId, Video video, CancellationToken cancellationToken = default);
        Task SetManyCacheAsync(Guid? requesterId, List<Video> avaliableVideos, CancellationToken cancellationToken = default);
    }
    public class VideoAccessCacheService : ITransientDependency, IVideoAccessCacheService
    {
        private readonly IDistributedCache<VideoAccessCacheItem, VideoAccessCacheKey> _cache;
        private readonly IVideoRepository _videoRepository;
        public VideoAccessCacheService(IDistributedCache<VideoAccessCacheItem, VideoAccessCacheKey> cache, IVideoRepository videoRepository)
        {
            _cache = cache;
            _videoRepository = videoRepository;
        }

        private DistributedCacheEntryOptions GetOptions()
        {
            return new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
            };
        }
        public virtual async Task SetManyCacheAsync(Guid? requesterId, List<Video> avaliableVideos, CancellationToken cancellationToken = default)
        {
            KeyValuePair<VideoAccessCacheKey, VideoAccessCacheItem> CachePairs(Video entity)
            {
                var key = new VideoAccessCacheKey()
                {
                    RequesterId = requesterId,
                    VideoId = entity.Id
                };
                var value = new VideoAccessCacheItem()
                {
                    HasAccess = true
                };
                return new KeyValuePair<VideoAccessCacheKey, VideoAccessCacheItem>(key, value);
            }

            await _cache.SetManyAsync(avaliableVideos.Select(CachePairs), GetOptions(), false, true, cancellationToken);
        }
        public virtual async Task CheckAccessAsync(Guid? requesterId, Video video, CancellationToken cancellationToken = default)
        {
            var key = new VideoAccessCacheKey()
            {
                RequesterId = requesterId,
                VideoId = video.Id
            };
            var cacheItem = await _cache.GetAsync(key);
            if (cacheItem is null)
            {
                cacheItem = new VideoAccessCacheItem() { HasAccess = await _videoRepository.HasAccessAsync(requesterId, video) };
                await _cache.SetAsync(key, cacheItem, GetOptions(), false, true, cancellationToken);
            }
            if (!cacheItem.HasAccess)
            {
                throw new InvalidOperationException();
            }
        }

    }
}
