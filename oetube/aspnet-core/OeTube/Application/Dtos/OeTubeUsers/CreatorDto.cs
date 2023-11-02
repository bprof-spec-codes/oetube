using OeTube.Application.Services.Url;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.OeTubeUsers
{
    public interface IMayHaveCreatorDto
    {
        CreatorDto? Creator { get; set; }
    }
    public class CreatorDtoMapper : IObjectMapper<OeTubeUser, CreatorDto>,ITransientDependency
    {
        private readonly IImageUrlService _urlService;

        public CreatorDtoMapper(UserUrlService urlService)
        {
            _urlService = urlService;
        }

        public CreatorDto Map(OeTubeUser source)
        {
            return Map(source, new CreatorDto());
        }

        public CreatorDto Map(OeTubeUser source, CreatorDto destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.ThumbnailImage = _urlService.GetThumbnailImageUrl(source.Id);
            return destination;
        }
    }
    public class CreatorDto:EntityDto<Guid>
    {
        
        public string Name { get; set; } = string.Empty;
        public string ThumbnailImage { get; set; } = string.Empty;
    }
}