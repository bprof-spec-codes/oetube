using Microsoft.AspNetCore.Mvc;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.Videos;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.Videos;
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
        public VideoAppService(VideoManager repository) : base(repository)
        {
        }

        public async Task<VideoUploadStateDto> StartUploadAsync(StartVideoUploadDto input)
        {
            return await CreateAsync<Video, VideoUploadStateDto>(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input.Content);
                return await Repository.StartUploadAsync(input.Name, input.Description, input.Access, CurrentUser.Id, content);
            });
        }

        public async Task<VideoUploadStateDto> ContinueUploadAsync(Guid id, IRemoteStreamContent input)
        {
            var video = await Repository.GetAsync(id);
            return await UpdateAsync<Video, VideoUploadStateDto>(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input);
                return await Repository.ContinueUploadAsync(video, (ByteContent)content);
            }, video);
        }

        [HttpGet("api/app/video/{id}/{width}x{height}/list.m3u8")]
        public async Task<IRemoteStreamContent> GetHlsListAsync(Guid id, int width, int height)
        {
            await CheckPolicyAsync(GetPolicy);
            var content = await Repository.Files.ReadHlsListAsync(id, new Resolution(width, height));
            return content.GetRemoteStreamContent();
        }

        [HttpGet("api/app/video/{id}/{width}x{height}/{segment}.ts")]
        public async Task<IRemoteStreamContent> GetHlsSegmentAsync(Guid id, int width, int height, int segment)
        {
            await CheckPolicyAsync(GetPolicy);
            var content = await Repository.Files.ReadHlsSegmentAsync(id, new Resolution(width, height), segment);
            return content.GetRemoteStreamContent();
        }

        [HttpGet("api/app/video/{id}/index_image")]
        public async Task<IRemoteStreamContent> GetIndexImageAsync(Guid id)
        {
            await CheckPolicyAsync(GetPolicy);
            var content = await Repository.Files.ReadSelectedFrameAsync(id);
            return content.GetRemoteStreamContent();
        }

        public async Task SelectIndexImageAsync(Guid id, int index)
        {
            await CheckPolicyAsync(UpdatePolicy);
            var video = await Repository.GetAsync(id);
            CheckCreator(video);
            await Repository.SelectFrameAsync(id, index);
        }

        public async Task<PagedResultDto<GroupListItemDto>> GetAccessGroupsAsync(Guid id, GroupQueryDto input)
        {
            var video = await Repository.GetAsync(id);
            return await GetListAsync<Group, GroupListItemDto>
                (async () => await Repository.GetAccessGroupsAsync(video, input));
        }

        public async Task<VideoDto> UpdateAsync(Guid id, UpdateVideoDto input)
        {
            return await UpdateAsync<VideoManager, Video, Guid, VideoDto, UpdateVideoDto>(Repository, id, input);
        }

        public async Task<VideoDto> UpdateAccessGroupsAsync(Guid id, UpdateAccessGroupsDto input)
        {
            var video = await Repository.GetAsync(id);
            return await UpdateAsync<Video, VideoDto>
                (async () => await Repository.UpdateAccessGroupsAsync(video, input.AccessGroups), video);
        }

        public async Task DeleteAsync(Guid id)
        {
            await DeleteAsync<VideoManager, Video, Guid>(Repository, id);
        }
    }
}