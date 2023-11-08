using OeTube.Application.Services.Url;
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
    public class CreatorDtoMapper : IObjectMapper<Guid?, CreatorDto?>,ITransientDependency
    {
        private readonly IImageUrlService _urlService;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        public CreatorDtoMapper(UserUrlService urlService, IUserRepository userRepository, ICurrentUser currentUser)
        {
            _urlService = urlService;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public CreatorDto? Map(Guid? source)
        {
            if(source is null)
            {
                return null;
            }
            else
            {
                return Map(source, new CreatorDto());
            }
        }

        public CreatorDto? Map(Guid? source, CreatorDto? destination)
        {
            if(source is null||destination is null)
            {
                return null;
            }
            else
            {
                var user = _userRepository.GetAsync(source.Value, false).Result;
                destination.Id = source.Value;
                destination.Name = user.Name;
                destination.ThumbnailImage = _urlService.GetThumbnailImageUrl(user.Id);
                destination.CurrentUserIsCreator = _currentUser.Id is not null && _currentUser.Id == source;
                return destination;
            }
        }
    }
    public class CreatorDto:EntityDto<Guid>
    {
        
        public string Name { get; set; } = string.Empty;
        public string ThumbnailImage { get; set; } = string.Empty;
        public bool CurrentUserIsCreator { get; set; }
    }
}