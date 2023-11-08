using Microsoft.AspNetCore.Mvc;
using OeTube.Application.Dtos;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.Videos;
using OeTube.Application.Services.Caches.VideoAccess;
using OeTube.Application.Services.Url;
using OeTube.Data.Repositories.Groups;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths.VideoFiles;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace OeTube.Application
{
    public class VideoAppService :
        ReadOnlyCreatorAppService<IVideoRepository, Video, Guid,VideoDto, VideoListItemDto, IVideoQueryArgs, VideoQueryDto>,
        IUpdateAppService<VideoDto, Guid, UpdateVideoDto>,
        IDeleteAppService<Guid>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IStartVideoUploadHandler _startVideoUpload;
        private readonly IContinueVideoUploadHandler _continueVideoUpload;
        private readonly ISelectVideoFrameHandler _selectVideoFrame;
        private readonly IVideoUrlService _urlService;
        private readonly IVideoAccessCacheService _videoAccessCache;
        public VideoAppService(IVideoRepository repository,
                               IFileContainerFactory fileContainerFactory,
                               IUserRepository userRepository,
                               IStartVideoUploadHandler startVideoUpload,
                               IContinueVideoUploadHandler continueVideoUpload,
                               ISelectVideoFrameHandler selectVideoFrame,
                               IVideoUrlService urlService,
                               IGroupRepository groupRepository,
                               IVideoAccessCacheService videoAccessCache) :
            base(repository, fileContainerFactory, userRepository)
        {
            _startVideoUpload = startVideoUpload;
            _continueVideoUpload = continueVideoUpload;
            _selectVideoFrame = selectVideoFrame;
            _urlService = urlService;
            _groupRepository = groupRepository;
            _videoAccessCache = videoAccessCache;
        }

        public async Task<VideoUploadStateDto> StartUploadAsync(StartVideoUploadDto input)
        {
            return await CreateAsync<Video, VideoUploadStateDto>(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input.Content);
                var args = await Task.FromResult(ObjectMapper.Map<StartVideoUploadDto, StartVideoUploadHandlerArgs>(input));
                var video= await _startVideoUpload.HandleFileAsync<Video>(content,args);
                var groups = await _groupRepository.GetManyAsync(input.AccessGroups);
                return await Repository.UpdateChildEntitiesAsync(video, groups, true);
            });
        }

        public async Task<VideoUploadStateDto> ContinueUploadAsync(Guid id, IRemoteStreamContent input)
        {
            var video = await Repository.GetAsync(id);
            return await UpdateAsync<Video, VideoUploadStateDto>(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input);
                var args = new ContinueVideoUploadHandlerArgs(video);
                return await _continueVideoUpload.HandleFileAsync<Video>(content, args);
            }, video);
        }
        public override async Task<VideoDto> GetAsync(Guid id)
        {
            return await GetAsync<Video, VideoDto>(async () =>
            {
                var video = await Repository.GetAsync(id, true);
                await _videoAccessCache.CheckAccessAsync(CurrentUser.Id,video);
                return video;
            });
        }
        public override async Task<PaginationDto<VideoListItemDto>> GetListAsync(VideoQueryDto input)
        {
            return await GetListAsync<Video, VideoListItemDto>(async () =>
            {
                var result = await Repository.GetAvaliableAsync(CurrentUser.Id, input);
                await _videoAccessCache.SetManyCacheAsync(CurrentUser.Id, result.Items);
                return result;
            });
        }

        public async Task SelectIndexImageAsync(Guid id, int index)
        {
            var video = await Repository.GetAsync(id);
            var args = new SelectVideoFrameHandlerArgs(id, index);
            await UpdateAsync(async () => await _selectVideoFrame.HandleFileAsync<Video>(args) ,video);
        }
        public async Task<VideoIndexImagesDto> GetIndexImagesAsync(Guid id)
        {
            await CheckPolicyAsync(GetPolicy);
            var entity=await Repository.GetAsync(id,false);
            await _videoAccessCache.CheckAccessAsync(CurrentUser.Id, entity);
            
            var selected = _urlService.GetIndexImageUrl(id);
            var indexImages = new List<string>();
            int index = 1;
            do
            {
                var name = FileContainer.FindFile(new FramePath(id, index));
                if (name == null)
                {
                    break;
                }
                indexImages.Add(_urlService.GetIndexImageByIndexUrl(id, index));
                index++;
            }
            while (true);
            return new VideoIndexImagesDto()
            {
                Id=id,
                IndexImages = indexImages,
                Selected = selected
            };
        }

        public async Task<PaginationDto<GroupListItemDto>> GetAccessGroupsAsync(Guid id, GroupQueryDto input)
        {
            var video = await Repository.GetAsync(id);
            return await GetListAsync<Group, GroupListItemDto>
                (async () => await Repository.GetChildEntitiesAsync(video, input));
        }

        public async Task<VideoDto> UpdateAsync(Guid id, UpdateVideoDto input)
        {
            var video = ObjectMapper.Map(input, await Repository.GetAsync(id));

            return await UpdateAsync<Video,VideoDto>(async () =>
            {
                var groups = await _groupRepository.GetManyAsync(input.AccessGroups);
                await Repository.UpdateAsync(video);
                return await Repository.UpdateChildEntitiesAsync(video, groups,true);
            },video);
        }

        public async Task DeleteAsync(Guid id)
        {
            await DeleteAsync(Repository, id);
        }
  

        private async Task<IRemoteStreamContent?> GetFileOrNullAsync(Guid id,string name,Func<Task<ByteContent?>> getFileMethod)
        {
            var entity = await Repository.GetAsync(id, false);
            await _videoAccessCache.CheckAccessAsync(CurrentUser.Id,entity);
            name = $"{nameof(Video).ToLower()}_{id}_{name}";
            return await GetFileOrNullAsync(getFileMethod, name);
        }

        [HttpGet("api/src/video/{id}/{width}x{height}/list.m3u8")]
        public async Task<IRemoteStreamContent?> GetHlsListAsync(Guid id, int width, int height)
        {
            return await GetFileOrNullAsync(id, "list", async () =>
            {
                return await FileContainer.GetFileOrNullAsync(new HlsListPath(id, new Resolution(width, height)));
            });
        }

        [HttpGet("api/src/video/{id}/{width}x{height}/{segment}.ts")]
        public async Task<IRemoteStreamContent?> GetHlsSegmentAsync(Guid id, int width, int height, int segment)
        {
            return await GetFileOrNullAsync(id, segment.ToString(), async () =>
            {
                return await FileContainer.GetFileOrNullAsync(new HlsSegmentPath(id, new Resolution(width, height), segment));
            });
        }

        [HttpGet("api/src/video/{id}/index_image")]
        public async Task<IRemoteStreamContent?> GetIndexImageAsync(Guid id)
        {
            return await GetFileOrNullAsync(id, "index_image", async () =>
            {
                return await FileContainer.GetFileOrNullAsync(new SelectedFramePath(id));
            });
        }

        [HttpGet("api/src/video/{id}/index_image/{index}")]
        public async Task<IRemoteStreamContent?> GetIndexImageByIndexAsync(Guid id, int index)
        {
            return await GetFileOrNullAsync(id, $"index_image_{index}", async () =>
            {
                return await FileContainer.GetFileOrNullAsync(new FramePath(id, index));
            });
        }

    }
}