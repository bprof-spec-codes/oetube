﻿using OeTube.Application.Caches;
using OeTube.Application.Caches.Composite;
using OeTube.Application.Url;
using OeTube.Domain.Entities;
using OeTube.Domain.Repositories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.OeTubeUsers
{
    public interface IMayHaveCreatorDto
    {
        CreatorDto? Creator { get; set; }
    }

    public class CreatorDtoMapper : AsyncNewDestinationObjectMapper<Guid?, CreatorDto?>, ITransientDependency
    {
        private readonly UserUrlService _urlService;
        private readonly ICurrentUser _currentUser;
        private readonly UserCacheService _cacheService;

        public CreatorDtoMapper(UserUrlService urlService,ICurrentUser currentUser, UserCacheService cacheService)
        {
            _urlService = urlService;
            _currentUser = currentUser;
            _cacheService = cacheService;
        }

        public override async Task<CreatorDto?> MapAsync(Guid? source, CreatorDto? destination)
        {
            if (source is null || destination is null)
            {
                return null;
            }
            else
            {
                destination.Id = source.Value;
                destination.Name = await _cacheService.GetOrAddCreatorNameAsync(source.Value);
                destination.ThumbnailImage = _urlService.GetThumbnailImageUrl(source.Value);
                destination.CurrentUserIsCreator = _currentUser.Id is not null && _currentUser.Id == source;
                return destination;
            }
        }
    }
    public class CreatorDto : EntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string ThumbnailImage { get; set; } = string.Empty;
        public bool CurrentUserIsCreator { get; set; }
    }
}