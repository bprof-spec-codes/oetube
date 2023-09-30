using Microsoft.AspNetCore.Mvc;
using OeTube.Application.Services;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc;

namespace OeTube.Controllers
{
    [ApiController]
    [Route("api/app/video")]
    public class VideoStreamController:AbpControllerBase
    {
        private readonly VideoStreamService _service;

        public VideoStreamController(VideoStreamService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("src/{id}/{height}/list.m3u8")]
        public async Task<FileStreamResult> GetM3U8Async(Guid id, int height)
        {
            return await _service.GetM3U8Async(id, height);
        }

        [HttpGet]
        [Route("src/{id}/{height}/{segment}.ts")]
        public async Task<FileStreamResult> GetM3U8SegmentAsync(Guid id, int height, int segment)
        {
            return await _service.GetM3U8SegmentAsync(id, height, segment);
        }
    }
}
