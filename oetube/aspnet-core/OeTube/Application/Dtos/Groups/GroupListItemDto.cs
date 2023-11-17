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
    public class GroupItemMapper : AsyncNewDestinationObjectMapper<Group, GroupListItemDto>, ITransientDependency
    {
        private readonly GroupUrlService _urlService;
        private readonly CreatorDtoMapper _creatorMapper;
        private readonly GroupCacheService _cacheService;

        public GroupItemMapper(GroupUrlService urlService, CreatorDtoMapper creatorMapper, GroupCacheService cacheService, IGroupRepository repository)
        {
            _urlService = urlService;
            _creatorMapper = creatorMapper;
            _cacheService = cacheService;
        }

        public override async Task<GroupListItemDto> MapAsync(Group source, GroupListItemDto destination)
        {
            destination.Id = source.Id;
            destination.CreationTime = source.CreationTime;
            destination.Name = source.Name;
            destination.ThumbnailImage = _urlService.GetThumbnailImageUrl(source.Id);
            destination.CurrentUserIsMember = await _cacheService.GetOrAddCurrentUserIsMemberAsync(source);
            destination.TotalMembersCount = await _cacheService.GetOrAddMembersCountAsync(source);
            destination.Creator = await _creatorMapper.MapAsync(source.CreatorId);
            return destination;
        }
    }

    public class GroupListItemDto : EntityDto<Guid>, IMayHaveCreatorDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public string? ThumbnailImage { get; set; }
        public CreatorDto? Creator { get; set; }
        public bool CurrentUserIsMember { get; set; }
        public int TotalMembersCount { get; set; }
    }
}