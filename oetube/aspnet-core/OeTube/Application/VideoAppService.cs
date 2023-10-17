using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OeTube.Infrastructure.SignalR;
using OeTube.Infrastructure.VideoStorage;
using System.Net;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace OeTube.Application
{
    public class VideoAppService : ApplicationService
    {
        private readonly IVideoStorageService _videoStorageService;
        public VideoAppService(IVideoStorageService videoStorageService)
        {
            _videoStorageService = videoStorageService;

        }
        public async Task<Guid> UploadVideoAsync(IRemoteStreamContent content)
        {

            Guid id = GuidGenerator.Create();
            await _videoStorageService.SaveVideoAsync(id, content);
            return id;
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
