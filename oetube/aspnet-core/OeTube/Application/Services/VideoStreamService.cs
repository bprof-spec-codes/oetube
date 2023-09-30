using Microsoft.AspNetCore.Mvc;
using OeTube.Infrastructure.VideoStorage;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace OeTube.Application.Services
{
    [RemoteService(false)]
    public class VideoStreamService : ApplicationService
    {
        private readonly IVideoStorageService _videoStorageService;
        public VideoStreamService(IVideoStorageService videoStorageService)
        {
            _videoStorageService = videoStorageService;
        }
        public async Task<FileStreamResult> GetM3U8SegmentAsync(Guid id, int resolution, int segment)
        {
            return await _videoStorageService.GetM3U8SegmentAsync(id, resolution, segment);
        }
        public async Task<FileStreamResult> GetM3U8Async(Guid id, int resolution)
        {
            return await _videoStorageService.GetM3U8Async(id, resolution);
        }
    }
}
