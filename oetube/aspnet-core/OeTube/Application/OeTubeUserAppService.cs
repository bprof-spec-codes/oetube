using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OeTube.Application.Dtos;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Dtos.Videos;
using OeTube.Application.Services.Caches.VideoAccess;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths.ImageFiles;
using OeTube.Domain.Infrastructure;
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
    public class OeTubeUserAppService :
        ReadOnlyCustomAppService<IUserRepository, OeTubeUser, Guid,
            UserDto, UserListItemDto, IUserQueryArgs, UserQueryDto>,
        IUpdateAppService<UserDto, Guid, UpdateUserDto>
    {
        private readonly IImageUploadHandler _imageUploadHandler;
        private readonly IDefaultImageUploadHandler _defaultImageUploadHandler;
        private readonly IVideoAccessCacheService _videoAccessCache;

        public OeTubeUserAppService(IUserRepository repository,
                                    IFileContainerFactory fileContainerFactory,
                                    IImageUploadHandler imageUploadHandler,
                                    IDefaultImageUploadHandler defaultImageUploadHandler,
                                    IVideoAccessCacheService videoAccessCache) : base(repository, fileContainerFactory)
        {
            _imageUploadHandler = imageUploadHandler;
            _defaultImageUploadHandler = defaultImageUploadHandler;
            _videoAccessCache = videoAccessCache;
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input)
        {
            CheckCreator(id);
            return await UpdateAsync<OeTubeUser,Guid,UserDto,UpdateUserDto>(Repository, id, input);
        }

        public async Task UploadDefaultImageAsync(IRemoteStreamContent input)
        {
            await UpdateAsync(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input);
                await _defaultImageUploadHandler.HandleFileAsync<OeTubeUser>(content);
            });
        }


        public async Task UploadImageAsync(Guid id, IRemoteStreamContent input)
        {
            var user = await Repository.GetAsync(id);
            await UpdateAsync(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input);
                await _imageUploadHandler.HandleFileAsync<OeTubeUser>(content, new ImageUploadHandlerArgs(id));
            }, user);
        }


        public async Task<PaginationDto<GroupListItemDto>> GetJoinedGroupsAsync(Guid id, GroupQueryDto input)
        {
            var user = await Repository.GetAsync(id);
            return await GetListAsync<Group, GroupListItemDto>(async () => await Repository.GetJoinedGroupsAsync(id, input));
        }

        public async Task<PaginationDto<GroupListItemDto>> GetGroupsAsync(Guid id, GroupQueryDto input)
        {
            var user = await Repository.GetAsync(id);
            return await GetListAsync<Group, GroupListItemDto>(async () => await Repository.GetCreatedEntitiesAsync(id, input));
        }

        public async Task<PaginationDto<VideoListItemDto>> GetVideosAsync(Guid id, VideoQueryDto input)
        {
            return await GetListAsync<Video, VideoListItemDto>(async () =>
            {
                var videos = await Repository.GetAvaliableCreatedEntititesAsync(id,CurrentUser.Id, input, false);
                await _videoAccessCache.SetManyCacheAsync(CurrentUser.Id, videos.Items);
                return videos;
            });
        }
       

        [HttpGet("api/src/ou-tube-user/{id}/image")]
        public async Task<IRemoteStreamContent?> GetImageAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id, false);
            string name = $"{nameof(OeTubeUser).ToLower()}_{id}_image";
            return await GetFileAsync(async () => await FileContainer.GetFileOrDefault(new SourceImagePath(id)), name);
        }
        [HttpGet("api/src/ou-tube-user/{id}/thumbnail-image")]
        public async Task<IRemoteStreamContent?> GetThumbnailImageAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id, false);
            string name = $"{nameof(OeTubeUser).ToLower()}_{id}_thumbnail_image";
            return await GetFileAsync(async () => await FileContainer.GetFileOrDefault(new ThumbnailImagePath(id)), name);
        }

    }
}