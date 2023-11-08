using Microsoft.AspNetCore.Mvc;
using OeTube.Application.Dtos;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.FilePaths.ImageFiles;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace OeTube.Application
{
    public class GroupAppService :
        ReadOnlyCreatorAppService
        <IGroupRepository, Group, Guid, GroupDto, GroupListItemDto, IGroupQueryArgs, GroupQueryDto>,
        ICreateAppService<GroupDto, CreateUpdateGroupDto>,
        IUpdateAppService<GroupDto, Guid, CreateUpdateGroupDto>,
        IDeleteAppService<Guid>
    {
        private readonly IImageUploadHandler _imageUploadHandler;
        private readonly IDefaultImageUploadHandler _defaultImageUploadHandler;
        public GroupAppService(IGroupRepository repository,
                               IFileContainerFactory fileContainerFactory,
                               IUserRepository userRepository,
                               IImageUploadHandler imageUploadHandler,
                               IDefaultImageUploadHandler defaultImageUploadHandler) : base(repository, fileContainerFactory, userRepository)
        {
            _imageUploadHandler = imageUploadHandler;
            _defaultImageUploadHandler = defaultImageUploadHandler;
        }

        public async Task<GroupDto> CreateAsync(CreateUpdateGroupDto input)
        {
            return await CreateAsync<Group, Guid, GroupDto, CreateUpdateGroupDto>(Repository, input);
         
        }

        public async Task DeleteAsync(Guid id)
        {
            await DeleteAsync(Repository, id);
        }

        public async Task<GroupDto> UpdateAsync(Guid id, CreateUpdateGroupDto input)
        {
            return await UpdateAsync<Group, Guid, GroupDto, CreateUpdateGroupDto>(Repository, id, input);
        }

     
        public async Task UploadDefaultImageAsync(IRemoteStreamContent input)
        {
            await UpdateAsync(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input);
                await _defaultImageUploadHandler.HandleFileAsync<Group>(content);
            });
        }
        public async Task UploadImageAsync(Guid id,IRemoteStreamContent input)
        {
            var group = await Repository.GetAsync(id);
            await UpdateAsync(async () =>
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(input);
                await _imageUploadHandler.HandleFileAsync<Group>(content,new ImageUploadHandlerArgs(id));
            }, group);
        }

        public async Task<PaginationDto<UserListItemDto>> GetGroupMembersAsync(Guid id, UserQueryDto input)
        {
            var group = await Repository.GetAsync(id);
            return await GetListAsync<OeTubeUser, UserListItemDto>(() => Repository.GetChildEntitiesAsync(group, input));
        }
       
        [HttpGet("api/src/group/{id}/image")]
        public async Task<IRemoteStreamContent?> GetImageAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id, false);
            string name = $"{nameof(Group).ToLower()}_{id}_image";
            return await GetFileAsync(async () => await FileContainer.GetFileOrDefault(new SourceImagePath(id)));
        }
        [HttpGet("api/src/group/{id}/thumbnail-image")]
        public async Task<IRemoteStreamContent?> GetThumbnailImageAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id, false);
            string name = $"{nameof(Group).ToLower()}_{id}_thumbnail_image";
            return await GetFileAsync(async () => await FileContainer.GetFileOrDefault(new ThumbnailImagePath(id)), name);
        }
    }
}