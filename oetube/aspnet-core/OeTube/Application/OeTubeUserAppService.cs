using Microsoft.AspNetCore.Mvc;
using OeTube.Application.AuthorizationCheckers;
using OeTube.Application.Dtos;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Methods;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.FilePaths.ImageFiles;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application
{
    public class OeTubeUserAppService : IApplicationService, ITransientDependency
    {
        private readonly UserMethodFactory _factory;
        private readonly Type _ownerAth = typeof(OwnerChecker);

        public OeTubeUserAppService(UserMethodFactory factory)
        {
            _factory = factory;
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            return await _factory.CreateGetMethod<UserDto>().GetAsync(id);
        }

        public async Task<PaginationDto<UserListItemDto>> GetListAsync(UserQueryDto input)
        {
            return await _factory.CreateGetListMethod<UserListItemDto>()
                                 .GetListAsync(input);
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input)
        {
            return await _factory.CreateUpdateMethod<UpdateUserDto, UserDto>()
                                 .SetAuthorizationAndPolicy(_ownerAth)
                                 .UpdateAsync(id, input);
        }

        public async Task UploadDefaultImageAsync(IRemoteStreamContent input)
        {
            await _factory.CreateUploadDefaultFileMethod<IDefaultImageUploadHandler>()
                          .UploadFile(input);
        }

        public async Task<PaginationDto<GroupListItemDto>> GetGroupsAsync(Guid id, GroupQueryDto input)
        {
            return await _factory.CreateGetChildrenListMethod<Group, IGroupQueryArgs, GroupListItemDto>()
                           .GetChildrenListAsync(id, input);
        }

        [HttpGet("api/src/ou-tube-user/{id}/image")]
        public async Task<IRemoteStreamContent> GetImageAsync(Guid id)
        {
            return await _factory.CreateGetDefaultFileMethod<SourceImagePath>()
                                 .SetFileName("image")
                                 .GetFileAsync(id, new SourceImagePath(id));
        }

        [HttpGet("api/src/ou-tube-user/{id}/thumbnail-image")]
        public async Task<IRemoteStreamContent> GetThumbnailImageAsync(Guid id)
        {
            return await _factory.CreateGetDefaultFileMethod<ThumbnailImagePath>()
                                 .SetFileName("thumnail_image")
                                 .GetFileAsync(id, new ThumbnailImagePath(id));
        }
    }

    public class UserMethodFactory : ApplicationMethodFactory<IUserRepository, OeTubeUser, Guid, IUserQueryArgs>, ITransientDependency
    {
        public UserMethodFactory(IUserRepository repository, IAbpLazyServiceProvider serviceProvider, IFileContainerFactory fileContainerFactory) : base(repository, serviceProvider, fileContainerFactory)
        {
        }
    }
}