using Microsoft.AspNetCore.Mvc;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.Videos;
using OeTube.Application.Services.Url;
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
        private readonly IStartVideoUploadHandler _startVideoUpload;
        private readonly IContinueVideoUploadHandler _continueVideoUpload;
        private readonly ISelectVideoFrameHandler _selectVideoFrame;
        private readonly IVideoUrlService _urlService;
        public VideoAppService(IVideoRepository repository,
                               IFileContainerFactory fileContainerFactory,
                               IUserRepository userRepository,
                               IStartVideoUploadHandler startVideoUpload,
                               IContinueVideoUploadHandler continueVideoUpload,
                               ISelectVideoFrameHandler selectVideoFrame,
                               IVideoUrlService urlService) :
            base(repository, fileContainerFactory, userRepository)
        {
            _startVideoUpload = startVideoUpload;
            _continueVideoUpload = continueVideoUpload;
            _selectVideoFrame = selectVideoFrame;
            _urlService = urlService;
        }

        public async Task<VideoUploadStateDto> StartUploadAsync(StartVideoUploadDto input)
        {
            return await CreateAsync<Video, VideoUploadStateDto>(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input.Content);
                var args = await Task.FromResult(ObjectMapper.Map<StartVideoUploadDto, StartVideoUploadHandlerArgs>(input));

                return await _startVideoUpload.HandleFileAsync<Video>(content,args);
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

        public async Task SelectIndexImageAsync(Guid id, int index)
        {
            var video = await Repository.GetAsync(id);
            var args = new SelectVideoFrameHandlerArgs(id, index);
            await UpdateAsync(async () => await _selectVideoFrame.HandleFileAsync<Video>(args) ,video);
        }
        public async Task<VideoIndexImagesDto> GetIndexImagesAsync(Guid id)
        {
            await CheckPolicyAsync(GetPolicy);
            await GetAsync(id);
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

        public async Task<PagedResultDto<GroupListItemDto>> GetAccessGroupsAsync(Guid id, GroupQueryDto input)
        {
            var video = await Repository.GetAsync(id);
            return await GetListAsync<Group, GroupListItemDto>
                (async () => await Repository.GetAccessGroupsAsync(video, input));
        }

        public async Task<VideoDto> UpdateAsync(Guid id, UpdateVideoDto input)
        {
            return await UpdateAsync<Video, Guid, VideoDto, UpdateVideoDto>(Repository, id, input);
        }

        public async Task<VideoDto> UpdateAccessGroupsAsync(Guid id, UpdateAccessGroupsDto input)
        {
            var video = await Repository.GetAsync(id);
            return await UpdateAsync<Video, VideoDto>
                (async () => await Repository.UpdateAccessGroupsAsync(video, input.AccessGroups), video);
        }
        public async Task DeleteAsync(Guid id)
        {
            await DeleteAsync(Repository, id);
        }

        [HttpGet("api/src/video/{id}/{width}x{height}/list.m3u8")]
        public async Task<IRemoteStreamContent?> GetHlsListAsync(Guid id, int width, int height)
        {
            var entity = Repository.GetAsync(id, false);
            string name = $"{nameof(Video).ToLower()}_{id}_list";
            return await GetFileOrNullAsync(async () => await FileContainer.GetFileOrNullAsync(new HlsListPath(id, new Resolution(width, height))),name);
        }

        [HttpGet("api/src/video/{id}/{width}x{height}/{segment}.ts")]
        public async Task<IRemoteStreamContent?> GetHlsSegmentAsync(Guid id, int width, int height, int segment)
        {
            var entity = Repository.GetAsync(id, false);
            string name = $"{nameof(Video).ToLower()}_{id}_{segment}";
            return await GetFileOrNullAsync(async () => await FileContainer.GetFileOrNullAsync(new HlsSegmentPath(id, new Resolution(width, height), segment)),name);
        }

        [HttpGet("api/src/video/{id}/index_image")]
        public async Task<IRemoteStreamContent?> GetIndexImageAsync(Guid id)
        {
            var entity = Repository.GetAsync(id, false);
            string name = $"{nameof(Video).ToLower()}_{id}_index_image";
            return await GetFileOrNullAsync(async () => await FileContainer.GetFileOrNullAsync(new SelectedFramePath(id)),name);
        }

        [HttpGet("api/src/video/{id}/index_image/{index}")]
        public async Task<IRemoteStreamContent?> GetIndexImageByIndexAsync(Guid id, int index)
        {
            var entity = Repository.GetAsync(id, false);
            string name = $"{nameof(Video).ToLower()}_{id}_index_image_{index}";
            return await GetFileOrNullAsync(async () => await FileContainer.GetFileOrNullAsync(new FramePath(id, index)),name);
        }

    }
}