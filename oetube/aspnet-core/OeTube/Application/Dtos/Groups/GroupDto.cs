using OeTube.Application.Services.Url;
using OeTube.Domain.Entities.Groups;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.Groups
{
    public class GroupMapper : IObjectMapper<Group, GroupDto>, ITransientDependency
    {
        private readonly IUrlService _urlService;
        public GroupMapper(IUrlService urlService)
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
            destination.CreatorId = source.CreatorId;
            destination.Description = source.Description;
            destination.EmailDomains = source.EmailDomains.Select(ed => ed.Domain).ToList();
            destination.Members = source.Members.Select(m => m.UserId).ToList();
            destination.ImageSource = _urlService.GetUrl<GroupAppService>(nameof(GroupAppService.GetImageAsync), new RouteTemplateParameter(source.Id));
            return destination;
        }
    }

    public class GroupDto : EntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public List<string> EmailDomains { get; set; } = new List<string>();
        public List<Guid> Members { get; set; } = new List<Guid>();
        public string? ImageSource { get; set; } 
    }
}