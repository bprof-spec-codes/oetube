using Microsoft.AspNetCore.SignalR;
using OeTube.Infrastructure.SignalR;
using OeTube.Infrastructure.VideoStorage;
using System.Net;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace OeTube.Application.Services
{
    public class VideoAppService:ApplicationService
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
    }
}
