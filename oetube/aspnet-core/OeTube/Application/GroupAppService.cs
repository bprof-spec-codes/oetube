﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OeTube.Application.AuthorizationCheckers;
using OeTube.Application.Dtos;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Methods;
using OeTube.Data.Repositories.Groups;
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
    public class GroupAppService : IApplicationService, ITransientDependency
    {
        private readonly GroupMethodFactory _factory;
        private readonly Type _creatorAuth = typeof(CreatorChecker);

        public GroupAppService(GroupMethodFactory factory)
        {
            _factory = factory;
        }

        public async Task<GroupDto> GetAsync(Guid id)
        {
            return await _factory.CreateGetMethod<GroupDto>().GetAsync(id);
        }
    
        public async Task<PaginationDto<GroupListItemDto>> GetListAsync(GroupQueryDto input)
        {
            return await _factory.CreateGetListMethod<GroupListItemDto>()
                                 .GetListAsync(input);
        }

        [Authorize]
        public async Task<GroupDto> CreateAsync(CreateUpdateGroupDto input)
        {
            return await _factory.CreateCreateMethod<CreateUpdateGroupDto, GroupDto>()
                                 .CreateAsync(input);
        }

        [Authorize]
        public async Task DeleteAsync(Guid id)
        {
            await _factory.CreateDeleteMethod()
                          .SetAuthorizationAndPolicy(_creatorAuth)
                          .DeleteAsync(id);
        }

        [Authorize]
        public async Task<GroupDto> UpdateAsync(Guid id, CreateUpdateGroupDto input)
        {
            return await _factory.CreateUpdateMethod<CreateUpdateGroupDto, GroupDto>()
                                 .SetAuthorizationAndPolicy(_creatorAuth)
                                 .UpdateAsync(id, input);
        }

        [Authorize(Roles = "admin")]
        public async Task UploadDefaultImageAsync(IRemoteStreamContent input)
        {
            await _factory.CreateUploadDefaultFileMethod<IDefaultImageUploadHandler>()
                                  .UploadFile(input);
        }
        public async Task<PaginationDto<UserListItemDto>> GetExplicitMembers(Guid id, UserQueryDto input)
        {
            return await _factory.CreateGetChildrenListMethod<OeTubeUser, IUserQueryArgs, UserListItemDto>()
                                  .GetChildrenListAsync(id, input);
        }
        public async Task<PaginationDto<UserListItemDto>> GetGroupMembersAsync(Guid id, UserQueryDto input)
        {
            return await _factory.CreateGetChildrenListMethod<OeTubeUser, IUserQueryArgs, UserListItemDto>()
                                 .GetCustomChildrenListAsync<GroupRepository>(id,(repository,entity)=>repository.GetMembers(entity,input));
        }
        [HttpGet("api/src/group/default-image")]
        public async Task<IRemoteStreamContent> GetDefaultImageAsync()
        {
            return await _factory.CreateGetDefaultFileMethod<SourceImagePath>().GetDefaultFileAsync();
        }
        [HttpGet("api/src/group/{id}/image")]
        public async Task<IRemoteStreamContent> GetImageAsync(Guid id)
        {
            return await _factory.CreateGetDefaultFileMethod<SourceImagePath>()
                                 .SetFileName("image")
                                 .GetFileAsync(id, new SourceImagePath(id));
        }

        [HttpGet("api/src/group/{id}/thumbnail-image")]
        public async Task<IRemoteStreamContent> GetThumbnailImageAsync(Guid id)
        {
            return await _factory.CreateGetDefaultFileMethod<ThumbnailImagePath>()
                                 .SetFileName("thumbnail_image")
                                 .GetFileAsync(id, new ThumbnailImagePath(id));
        }
    }

    public class GroupMethodFactory : ApplicationMethodFactory<IGroupRepository, Group, Guid, IGroupQueryArgs>, ITransientDependency
    {
        public GroupMethodFactory(IGroupRepository repository, IAbpLazyServiceProvider serviceProvider, IFileContainerFactory fileContainerFactory) : base(repository, serviceProvider, fileContainerFactory)
        {
        }
    }
}