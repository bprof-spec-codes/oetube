using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OeTube.Application.Dtos.Videos;
using OeTube.Data.Repositories;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Infrastructure;
using OeTube.Infrastructure.FFmpeg;
using OeTube.Infrastructure.FFprobe;
using OeTube.Infrastructure.FFprobe.Infos;
using OeTube.Infrastructure.SignalR;
using OeTube.Infrastructure.VideoFileManager;
using OeTube.Infrastructure.VideoStorage;
using System.Net;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace OeTube.Application
{

    public class VideoAppService : ApplicationService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IVideoFileManager _videoFileManager;
        private readonly IFFProbeService _ffprobe;
        private readonly VideoStorage _videoStorage;

        public VideoAppService(
            IFFProbeService ffprobe,
            VideoStorage videoStorage,
            IVideoRepository videoRepository,
            IVideoFileManager videoFileManager)
        {
            _ffprobe = ffprobe;
            _videoStorage = videoStorage;
            _videoRepository = videoRepository;
            _videoFileManager = videoFileManager;
        }
        public async Task<VideoUploadStateDto> StartUploadAsync(StartVideoUploadDto input)
        {
            var content = await ByteContent.FromRemoteStreamContentAsync(input.Content); 
            var sourceInfo = await _ffprobe.AnalyzeAsync(content);
            _videoFileManager.ValidateSourceVideo(sourceInfo);
            
            var firstVideoStream = sourceInfo.VideoStreams[0];
            var resolution = firstVideoStream.Resolution;
            var format = sourceInfo.Format;
            var duration = sourceInfo.Duration;
            var desiredResolutions = _videoFileManager.GetDesiredResolutions(resolution);

            var video = new Video(GuidGenerator.Create(),
                                input.Name, CurrentUser?.Id,
                                format,
                                _videoFileManager.OutputFormat,
                                duration,
                                desiredResolutions)
                               .SetDescription(input.Description);

            await _videoStorage.SaveSourceAsync(video.Id,content);
            if (_videoFileManager.IsInDesiredResolutionAndFormat(video, firstVideoStream))
            {
                await _videoStorage.SaveResizedAsync(video.Id,resolution,content);
                video.Resolutions.Get(resolution).MarkReady();
            }

            await _videoRepository.InsertAsync(video, true);
            return new VideoUploadStateDto()
            {
                Id = video.Id,
                OutputFormat=video.OutputFormat,
                RemainingTasks = _videoFileManager.CreateUploadTasks(video)
            };
        }

        public async Task<VideoUploadStateDto> ContinueUploadAsync(Guid id, IRemoteStreamContent input)
        {
            var content = await ByteContent.FromRemoteStreamContentAsync(input);
            var video = await _videoRepository.GetAsync(id);
            var sourceInfo = await _ffprobe.AnalyzeAsync(await _videoStorage.GetSourceAsync(id));
            var resizedInfo = await _ffprobe.AnalyzeAsync(content);

            _videoFileManager.ValidateResizedVideo(video, sourceInfo, resizedInfo);
            var resolution = resizedInfo.VideoStreams[0].Resolution;
            
            await _videoStorage.SaveResizedAsync(video.Id,resolution,content);
            
            video.Resolutions.Get(resolution).MarkReady();

            await _videoRepository.UpdateAsync(video, true);
            return new VideoUploadStateDto()
            {
                Id = video.Id,
                RemainingTasks = _videoFileManager.CreateUploadTasks(video)
            };
        }

        /*
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
        }*/
    }
}
