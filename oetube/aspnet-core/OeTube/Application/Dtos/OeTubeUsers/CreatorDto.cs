using OeTube.Application.Caches;
using OeTube.Application.Url;
using OeTube.Data.Repositories.Users;
using OeTube.Domain.Entities;
using OeTube.Domain.Repositories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.OeTubeUsers
{
    public interface IMayHaveCreatorDto
    {
        CreatorDto? Creator { get; set; }
    }
    public class CreatorDtoMapper : AsyncNewDestinationObjectMapper<Guid?, CreatorDto?>,ITransientDependency
    {
        private readonly UserUrlService _urlService;
        private readonly ICurrentUser _currentUser;
        private readonly UserCacheService _cacheService;

        public CreatorDtoMapper(UserUrlService urlService, IUserRepository userRepository, ICurrentUser currentUser,UserCacheService cacheService)
        {
            _urlService = urlService;
            _currentUser = currentUser;
            _cacheService = cacheService.ConfigureCreatorName(userRepository);
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
                destination.Name =await _cacheService.GetOrAddCreatorNameAsync(source.Value);
                destination.ThumbnailImage = _urlService.GetThumbnailImageUrl(source.Value);
                destination.CurrentUserIsCreator = _currentUser.Id is not null && _currentUser.Id == source;
                return destination;
            }
        }
    }
    public static class CreatorCacheExtension
    {
        public static UserCacheService ConfigureCreatorName(this UserCacheService cacheService,IUserRepository repository)
        {
            cacheService.GlobalDtoCache.ConfigureProperty<CreatorDto,string>(c => c.Name, async (key, user, currentUserId) =>
            {
                user = await repository.GetAsync(key);
                return user.Name;
            },TimeSpan.FromMinutes(10));
            return cacheService;
        }
        public static async Task<string> GetOrAddCreatorNameAsync(this UserCacheService cacheService,Guid id)
        {
            return (await cacheService.GlobalDtoCache.GetOrAddAsync<CreatorDto, string>(id, null, c => c.Name))!;
        }
        public static async Task DeleteCreatorNameAsync(this UserCacheService cacheService, OeTubeUser user)
        {
            await cacheService.GlobalDtoCache.DeleteAsync<CreatorDto,string>(user, c => c.Name);
        }
    }

    public class CreatorDto:EntityDto<Guid>
    {
        
        public string Name { get; set; } = string.Empty;
        public string ThumbnailImage { get; set; } = string.Empty;
        public bool CurrentUserIsCreator { get; set; }
    }
}