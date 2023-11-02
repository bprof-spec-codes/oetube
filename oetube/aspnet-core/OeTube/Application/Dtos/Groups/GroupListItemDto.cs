using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Services.Url;
using OeTube.Domain.Entities.Groups;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.Groups
{
    public class GroupItemMapper : IObjectMapper<Group, GroupListItemDto>, ITransientDependency
    {
        private readonly IImageUrlService _urlService;

        public GroupItemMapper(GroupUrlService urlService)
        {
            _urlService = urlService;
        }

        public GroupListItemDto Map(Group source)
        {
            return Map(source, new GroupListItemDto());
        }

        public GroupListItemDto Map(Group source, GroupListItemDto destination)
        {
            destination.Id = source.Id;
            destination.CreationTime = source.CreationTime;
            destination.Name = source.Name;
            destination.ThumbnailImage= _urlService.GetThumbnailImageUrl(source.Id);
            return destination;
        }
    }

    public class GroupListItemDto : EntityDto<Guid>,IMayHaveCreatorDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public string? ThumbnailImage { get; set; }
        public CreatorDto? Creator { get; set; }
    }
}