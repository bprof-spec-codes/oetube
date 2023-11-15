using OeTube.Application.Caches;
using OeTube.Application.Caches.Composite;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Url;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Dtos.Groups
{
    public class GroupMapper : AsyncNewDestinationObjectMapper<Group, GroupDto>, ITransientDependency
    {
        private readonly GroupUrlService _urlService;
        private readonly CreatorDtoMapper _creatorMapper;
        private readonly GroupCacheService _cacheService;

        public GroupMapper(GroupUrlService urlService, CreatorDtoMapper creatorMapper, GroupCacheService cacheService)
        {
            _urlService = urlService;
            _creatorMapper = creatorMapper;
            _cacheService = cacheService;
        }

        public override async Task<GroupDto> MapAsync(Group source, GroupDto destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.CreationTime = source.CreationTime;
            destination.Description = source.Description;
            destination.EmailDomains = source.EmailDomains.Select(ed => ed.Domain).ToList();
            destination.Members = source.Members.Select(m => m.UserId).ToList();
            destination.Image = _urlService.GetImageUrl(source.Id);
            destination.Creator = await _creatorMapper.MapAsync(source.CreatorId);
            destination.CurrentUserIsMember = await _cacheService.GetOrAddCurrentUserIsMemberAsync(source);
            destination.TotalMembersCount = await _cacheService.GetOrAddMembersCountAsync(source);

            return destination;
        }
    }

    public class GroupDto : EntityDto<Guid>, IMayHaveCreatorDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreationTime { get; set; }
        public List<string> EmailDomains { get; set; } = new List<string>();
        public List<Guid> Members { get; set; } = new List<Guid>();
        public string? Image { get; set; }
        public CreatorDto? Creator { get; set; }
        public bool CurrentUserIsMember { get; set; }
        public int TotalMembersCount { get; set; }
    }
}