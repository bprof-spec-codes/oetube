using Microsoft.AspNetCore.Mvc;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.Videos;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Infrastructure.Videos.VideoFiles;
using OeTube.Domain.Managers;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace OeTube.Application
{
    public class VideoAppService :
        ReadOnlyCustomAppService<VideoManager, Video, Guid, VideoDto, VideoListItemDto, IVideoQueryArgs, VideoQueryDto>,
        IUpdateAppService<VideoDto, Guid, UpdateVideoDto>,
        IDeleteAppService<Guid>
    {
        public VideoAppService(VideoManager manager) : base(manager)
        {
        }

        public async Task<VideoUploadStateDto> StartUploadAsync(StartVideoUploadDto input)
        {
            return await CreateAsync<Video, VideoUploadStateDto>(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input.Content);
                return await Manager.StartUploadAsync(input.Name, input.Description, input.Access, CurrentUser.Id, content);
            });
        }

        public async Task<VideoUploadStateDto> ContinueUploadAsync(Guid id, IRemoteStreamContent input)
        {
            var video = await Manager.GetAsync(id);
            return await UpdateAsync<Video, VideoUploadStateDto>(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input);
                return await Manager.ContinueUploadAsync(video, content);
            }, video);
        }

        [HttpGet("api/app/video/{id}/{width}x{height}/list.m3u8")]
        public async Task<IRemoteStreamContent?> GetHlsListAsync(Guid id, int width, int height)
        {
            return await GetFileOrNullAsync(async () =>await Manager.GetFileOrNullAsync(new  HlsListFileClass(id, new Resolution(width, height))));
        }

        [HttpGet("api/app/video/{id}/{width}x{height}/{segment}.ts")]
        public async Task<IRemoteStreamContent?> GetHlsSegmentAsync(Guid id, int width, int height, int segment)
        {
            return await GetFileOrNullAsync(async () => await Manager.GetFileOrNullAsync(new HlsSegmentFileClass(id,new Resolution(width,height),segment)));
        }

        [HttpGet("api/app/video/{id}/index_image")]
        public async Task<IRemoteStreamContent?> GetIndexImageAsync(Guid id)
        {
            return await GetFileOrNullAsync(async () => await Manager.GetFileOrNullAsync(new SelectedFrameFileClass(id)));
        }

        public async Task SelectIndexImageAsync(Guid id, int index)
        {
            var video = await Manager.GetAsync(id);
            await UpdateAsync(async () => await Manager.SelectFrameAsync(id, index),video);
        }

        public async Task<PagedResultDto<GroupListItemDto>> GetAccessGroupsAsync(Guid id, GroupQueryDto input)
        {
            var video = await Manager.GetAsync(id);
            return await GetListAsync<Group, GroupListItemDto>
                (async () => await Manager.GetAccessGroupsAsync(video, input));
        }

        public async Task<VideoDto> UpdateAsync(Guid id, UpdateVideoDto input)
        {
            return await UpdateAsync<VideoManager, Video, Guid, VideoDto, UpdateVideoDto>(Manager, id, input);
        }

        public async Task<VideoDto> UpdateAccessGroupsAsync(Guid id, UpdateAccessGroupsDto input)
        {
            var video = await Manager.GetAsync(id);
            return await UpdateAsync<Video, VideoDto>
                (async () => await Manager.UpdateAccessGroupsAsync(video, input.AccessGroups), video);
        }
        public async Task DeleteAsync(Guid id)
        {
            await DeleteAsync<VideoManager, Video, Guid>(Manager, id);
        }
    }
}