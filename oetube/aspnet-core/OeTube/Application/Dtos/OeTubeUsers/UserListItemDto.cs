using OeTube.Application.Services.Url;
using OeTube.Domain.Entities;
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
            destination.CreationTime = source.CreationTime;
            destination.ThumbnailImageSource = _urlService.GetThumbnailImageUrl( source.Id);

            return destination;
        }
    }

    public class UserListItemDto : EntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public string EmailDomain { get; set; } = string.Empty;
        public string? ThumbnailImageSource { get; set; }
    }
}