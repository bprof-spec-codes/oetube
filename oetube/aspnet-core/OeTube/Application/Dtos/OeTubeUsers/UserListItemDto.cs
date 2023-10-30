using OeTube.Application.Services.Url;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.OeTubeUsers
{
    public class UserListItemMapper : IObjectMapper<OeTubeUser, UserListItemDto>, ITransientDependency
    {
        private readonly IImageUrlService _urlService;

        public UserListItemMapper(UserUrlService urlService)
        {
            _urlService = urlService;
        }

        public UserListItemDto Map(OeTubeUser source)
        {
            return Map(source, new UserListItemDto());
        }

        public UserListItemDto Map(OeTubeUser source, UserListItemDto destination)
        {
            destination.Id = source.Id;
            destination.EmailDomain = source.EmailDomain;
            destination.Name = source.Name;
            destination.RegistrationDate = source.CreationTime;
            destination.ImageSource = _urlService.GetImageUrl( source.Id);

            return destination;
        }
    }

    public class UserListItemDto : EntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public string EmailDomain { get; set; } = string.Empty;
        public string? ImageSource { get; set; }
    }
}