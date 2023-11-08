using OeTube.Application.Services.Url;
using OeTube.Domain.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.OeTubeUsers
{
    public class UserMapper : IObjectMapper<OeTubeUser, UserDto>, ITransientDependency
    {
        private readonly IImageUrlService _urlService;

        public UserMapper(UserUrlService urlService)
        {
            _urlService = urlService;
        }

        public UserDto Map(OeTubeUser source)
        {
            return Map(source, new UserDto());
        }

        public UserDto Map(OeTubeUser source, UserDto destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.CreationTime = source.CreationTime;
            destination.Image = _urlService.GetImageUrl(source.Id);
            return destination;
        }
    }

    public class UserDto : EntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? AboutMe { get; set; }
        public string EmailDomain { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public string? Image { get; set; }
    }
}