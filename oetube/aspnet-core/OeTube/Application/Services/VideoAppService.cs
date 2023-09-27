using OeTube.Domain.Storages;
using System.Net;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace OeTube.Application.Services
{
    public class VideoAppService:ApplicationService
    {
        private readonly VideoStorageService _videoStorageService;

        public VideoAppService(VideoStorageService videoStorageService)
        {
            _videoStorageService = videoStorageService;
        }

        public async Task UploadVideoAsync(IRemoteStreamContent content)
        {
             Guid id = GuidGenerator.Create();
            await _videoStorageService.SaveVideoAsync(id, content);
        }
    }
}
