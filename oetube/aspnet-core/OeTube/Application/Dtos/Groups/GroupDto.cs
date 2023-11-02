using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Services.Url;
using OeTube.Domain.Entities.Groups;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.Groups
{
    public class GroupMapper : IObjectMapper<Group, GroupDto>, ITransientDependency
    {
        private readonly IImageUrlService _urlService;
        public GroupMapper(GroupUrlService urlService)
        {
            _urlService = urlService;
        }
        public GroupDto Map(Group source)
        {
            return Map(source, new GroupDto());
        }

        public GroupDto Map(Group source, GroupDto destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.CreationTime = source.CreationTime;
            destination.Description = source.Description;
            destination.EmailDomains = source.EmailDomains.Select(ed => ed.Domain).ToList();
            destination.Members = source.Members.Select(m => m.UserId).ToList();
            destination.Image = _urlService.GetImageUrl(source.Id);
            return destination;
        }
    }

    public class GroupDto : EntityDto<Guid>,IMayHaveCreatorDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreationTime { get; set; }
        public List<string> EmailDomains { get; set; } = new List<string>();
        public List<Guid> Members { get; set; } = new List<Guid>();
        public string? Image { get; set; }
        public CreatorDto? Creator { get; set; }
    }
}