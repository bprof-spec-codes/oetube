﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OeTube.Application.AuthorizationCheckers;
using OeTube.Application.Dtos;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Dtos.Videos;
using OeTube.Application.Methods;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.FilePaths.ImageFiles;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(description: "Lekérdez egy User-t id alapján.")]
        public async Task<UserDto> GetAsync(Guid id)
        {
            return await _factory.CreateGetMethod<UserDto>().GetAsync(id);
        }
  
        [SwaggerOperation(description:"Pagináltan lekérdez több User-t a megadott keresési argumentumok alapján.")]
        public async Task<PaginationDto<UserListItemDto>> GetListAsync(UserQueryDto input)
        {
            return await _factory.CreateGetListMethod<UserListItemDto>()
                                 .GetListAsync(input);
        }

        [Authorize]
        [SwaggerOperation(description:"Módosít egy User profilját. Csak a hozzátartozó felhasználó hajthatja végre.")]
        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input)
        {
            return await _factory.CreateUpdateMethod<UpdateUserDto, UserDto>()
                                 .SetAuthorizationAndPolicy(_ownerAth)
                                 .UpdateAsync(id, input);
        }

        [Authorize(Roles = "admin")]
        [SwaggerOperation(description:"Feltölt egy alapértelmezett képet a Userekhez. Csak admin jogosultságú felhasználó hajthatja végre.")]
        public async Task UploadDefaultImageAsync(IRemoteStreamContent input)
        {
            await _factory.CreateUploadDefaultFileMethod<IDefaultImageUploadHandler>()
                          .UploadFile(input);
        }
        [SwaggerOperation(description: "Pagináltan lekérdezi az összes Grouput, amihez User tartozik a keresési argumentumok alapján.")]
        public async Task<PaginationDto<GroupListItemDto>> GetGroupsAsync(Guid id, GroupQueryDto input)
        {
            return await _factory.CreateGetChildrenListMethod<Group, IGroupQueryArgs, GroupListItemDto>()
                           .GetChildrenListAsync(id, input);
        }
        [HttpGet("api/src/oe-tube-user/default-image")]
        [SwaggerOperation(description:"Lekéri a Userek alapértelmezett képét.")]
        public async Task<IRemoteStreamContent> GetDefaultImageAsync()
        {
            return await _factory.CreateGetDefaultFileMethod<SourceImagePath>().GetDefaultFileAsync();
        }
        [HttpGet("api/src/ou-tube-user/{id}/image")]
        [SwaggerOperation(description:"Lekéri az adott id-jű User képét.")]
        public async Task<IRemoteStreamContent> GetImageAsync(Guid id)
        {
            return await _factory.CreateGetDefaultFileMethod<SourceImagePath>()
                                 .SetFileName("image")
                                 .GetFileAsync(id, new SourceImagePath(id));
        }

        [HttpGet("api/src/ou-tube-user/{id}/thumbnail-image")]
        [SwaggerOperation(description:"Lekéri az adott id-jű User thumbnail képét.")]
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