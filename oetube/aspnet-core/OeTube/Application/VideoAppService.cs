using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SignalR;
using OeTube.Application.Dtos.Videos;
using OeTube.Application.Extensions;
using OeTube.Data.Repositories;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Infrastructure;
using OeTube.Infrastructure.FF;
using OeTube.Infrastructure.FF.Probe;
using OeTube.Infrastructure.FF.Probe.Infos;
using OeTube.Infrastructure.VideoFileManager;
using OeTube.Infrastructure.VideoStorages;
using Scriban.Runtime.Accessors;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application
{
    public struct RouteTemplateParameter
    {
        public string Name { get; }
        public object Value { get; }
        public RouteTemplateParameter(object value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            Value = value;
            Name = parameterName??string.Empty;
        }
    }

    public class UrlService:ITransientDependency
    {
        private readonly IHttpContextAccessor contextAccessor;

        public UrlService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }
        public string BaseUrl => $"{contextAccessor?.HttpContext?.Request.Scheme}://{contextAccessor?.HttpContext?.Request.Host}";
        public string GetUrl(string template, params RouteTemplateParameter[] parameters)
        {

            foreach (var item in parameters)
            {
                template = template.Replace("{" + item.Name + "}", item.Value.ToString());
            }
            return BaseUrl + "/" + template;
        }
        public string? GetUrl<T>(string methodName, params RouteTemplateParameter[] parameters)
        {
            var type = typeof(T);
            var attribute = type.GetMethod(methodName)?.GetCustomAttribute<HttpMethodAttribute>();
            if (attribute is null||attribute.Template is null)
            {
                return null;
            }

            return GetUrl(attribute.Template, parameters);
        }
    }
    public class VideoAppService : ApplicationService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IVideoFileManager _videoFileManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IFFProbeService _ffprobe;
       
        private readonly VideoStorage _videoStorage;
        private readonly UrlService _url;
        public VideoAppService(
            IFFProbeService ffprobe,
            VideoStorage videoStorage,
            IVideoRepository videoRepository,
            IVideoFileManager videoFileManager,
            IBackgroundJobManager backgroundJobManager,
            UrlService urlService)
            
        {
            _ffprobe = ffprobe;
            _videoStorage = videoStorage;
            _videoRepository = videoRepository;
            _videoFileManager = videoFileManager;
            _backgroundJobManager = backgroundJobManager;
            _url = urlService;
        }
        public async Task<VideoUploadStateDto> StartUploadAsync(StartVideoUploadDto input)
        {
            if(input.Content is null)
            {
                throw new ArgumentException(null, nameof(input));
            }

            var content = await ByteContent.FromRemoteStreamContentAsync(input.Content); 
            var sourceInfo = await _ffprobe.AnalyzeAsync(content);
            _videoFileManager.ValidateSourceVideo(sourceInfo);
            
            var sourceVideoStream = sourceInfo.VideoStreams[0];
            var resolution = sourceVideoStream.Resolution;

            var video = new Video(GuidGenerator.Create(),
                         input.Name, CurrentUser?.Id,
                         sourceInfo.Duration,
                         _videoFileManager.GetDesiredResolutions(resolution))
                        .SetDescription(input.Description);


            await _videoStorage.Save.SourceAsync(video.Id,content);
            if (_videoFileManager.IsInDesiredResolutionAndFormat(sourceInfo))
            {
                await _videoStorage.Save.ResizedAsync(video.Id,resolution,content);
                video.Resolutions.Get(resolution).MarkReady();
            }

            await _videoRepository.InsertAsync(video, true);
            await ProcessUploadIfIsItReadyAsync(video,sourceVideoStream);
            return CreateVideoUploadStateDto(video);
        }
        public async Task<VideoUploadStateDto> ContinueUploadAsync(Guid id, IRemoteStreamContent input)
        {
            var content = await ByteContent.FromRemoteStreamContentAsync(input);
            var video = await _videoRepository.GetAsync(id);
            var sourceInfo = await _ffprobe.AnalyzeAsync(await _videoStorage.Get.SourceAsync(id));
            var resizedInfo = await _ffprobe.AnalyzeAsync(content);

            _videoFileManager.ValidateResizedVideo(video, sourceInfo, resizedInfo);
            var resolution = resizedInfo.VideoStreams[0].Resolution;
            
            await _videoStorage.Save.ResizedAsync(video.Id,resolution,content);

            video.Resolutions.Get(resolution).MarkReady();


            await _videoRepository.UpdateAsync(video, true);
            await ProcessUploadIfIsItReadyAsync(video, sourceInfo.VideoStreams[0]);
            return CreateVideoUploadStateDto(video);
        }
        private VideoUploadStateDto CreateVideoUploadStateDto(Video video)
        {
            return new VideoUploadStateDto()
            {
                Id = video.Id,
                OutputFormat = _videoFileManager.OutputFormat,
                RemainingTasks = _videoFileManager.CreateUploadTasks(video)
            };
        }
        private async Task ProcessUploadIfIsItReadyAsync(Video video,VideoStreamInfo sourceVideoStream)
        {
            if (video.IsAllResolutionReady())
            {
                var resolutions = video.GetResolutionsBy(true).ToArray();
                var extractFrameTarget = resolutions.OrderByDescending(r => r.Height).First();

                await _backgroundJobManager.EnqueueAsync(new ProcessUploadTask(video.Id,
                                                                               resolutions,
                                                                               extractFrameTarget,
                                                                               video.IsAllResolutionReady(),
                                                                               sourceVideoStream.Frames));
            }

        }

        public async Task<PagedResultDto<VideoItemDto>> GetListAsync(VideoFilterDto filter, PagedAndSortedResultRequestDto input)
        {
            var result = await _videoRepository.GetCompletedVideosQueryableAsync(filter.Name);
            return await result.ToPagedResultDtoAsync((v) =>
            {
               return new VideoItemDto()
                {
                    Id = v.Id,
                    CreationTime = v.CreationTime,
                    CreatorId = v.CreatorId,
                    Name = v.Name,
                    Duration = v.Duration,
                    IndexImageSrc = GetIndexImageUrl(v.Id)
                };
            }, input);

        }
        public async Task<VideoDto> GetAsync(Guid id)
        {
            var video =await _videoRepository.GetAsync(id);
            var resolutions = video.GetResolutionsBy(true)
                                   .Select(r => new ResolutionSrcDto()
                                   {
                                       Resolution = r,
                                       Src = GetHlsListUrl(id, r.Width, r.Height)
                                   }).ToList();

            var videoDto = new VideoDto()
            {
                Id = video.Id,
                Name = video.Name,
                PlaylistId = null,
                CreationTime = video.CreationTime,
                ResolutionsSrc = resolutions,
                Duration = video.Duration,
                Description = video.Description,
                CreatorId = video.CreatorId,
                AccessGroups = video.AccessGroups.Select(a => a.GroupId).ToList(),
                IndexImageSrc=GetIndexImageUrl(video.Id),
                IsUploadCompleted=video.IsUploadCompleted
            };
            return videoDto;
        }


       
        [HttpGet("api/app/video/{id}/{width}x{height}/list.m3u8")]
        public async Task<IRemoteStreamContent> GetHlsListAsync(Guid id,int width, int height)
        {
            var content= await _videoStorage.Get.HlsListAsync(id, new Resolution(width, height));
            return content.GetRemoteStreamContent();
        }
        private string GetHlsListUrl(Guid id, int width, int height)
        {
            return _url.GetUrl<VideoAppService>(nameof(GetHlsListAsync),
                                                new(id),
                                                new(width),
                                                new(height)) ??
                throw new NullReferenceException(nameof(GetHlsListAsync));
        }
        [HttpGet("api/app/video/{id}/{width}x{height}/{segment}.ts")]
        public async Task<IRemoteStreamContent> GetHlsSegmentAsync(Guid id, int width, int height,int segment)
        {
            var content = await _videoStorage.Get.HlsSegmentAsync(id, new Resolution(width, height), segment);
            return content.GetRemoteStreamContent();
        }
        private string GetHlsSegmentUrl(Guid id, int width, int height,int segment)
        {
            return _url.GetUrl<VideoAppService>(nameof(GetHlsSegmentAsync),
                                                new(id),
                                                new(width),
                                                new(height),
                                                new(segment)) ??
                throw new NullReferenceException(nameof(GetHlsSegmentAsync));
        }

        [HttpGet("api/app/video/{id}/index_image")]
        public async Task<IRemoteStreamContent> GetIndexImageAsync(Guid id)
        {
            var content = await _videoStorage.Get.SelectedFrameAsync(id);
            return content.GetRemoteStreamContent();
        }
        private string GetIndexImageUrl(Guid id)
        {
            return _url.GetUrl<VideoAppService>(nameof(GetIndexImageAsync),
                                                new RouteTemplateParameter(id)) ??
                    throw new NullReferenceException(nameof(GetIndexImageAsync));
                                             
        }


    }
}
