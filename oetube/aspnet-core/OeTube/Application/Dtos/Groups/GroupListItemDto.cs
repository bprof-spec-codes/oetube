using JetBrains.Annotations;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Services.Caches.GroupCache;
using OeTube.Application.Services.Url;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.Groups
{
    public class GroupItemMapper : IObjectMapper<Group, GroupListItemDto>, ITransientDependency
    {
        private readonly IImageUrlService _urlService;
        private readonly ICurrentUser _currentUser;
        private readonly IObjectMapper<Guid?, CreatorDto?> _creatorMapper;
        private readonly IIsMemberCacheService _isMemberCache;
        private readonly IGroupMembersCountCacheService _groupMembersCountCache;
        public GroupItemMapper(GroupUrlService urlService,ICurrentUser currentUser, IObjectMapper<Guid?, CreatorDto?> creatorMapper, IGroupMembersCountCacheService groupMembersCountCache, IIsMemberCacheService isMemberCache)
        {
            _urlService = urlService;
            _currentUser = currentUser;
            _creatorMapper = creatorMapper;
            _groupMembersCountCache = groupMembersCountCache;
            _isMemberCache = isMemberCache;
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
            destination.CurrentUserIsMember = _isMemberCache.IsMemberAsync(_currentUser.Id, source).Result;
            destination.TotalMembersCount = _groupMembersCountCache.GetTotalCountAsync(source).Result;
            destination.Creator = _creatorMapper.Map(source.CreatorId);
            return destination;
        }
    }

    public class GroupListItemDto : EntityDto<Guid>,IMayHaveCreatorDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public string? ThumbnailImage { get; set; }
        public CreatorDto? Creator { get; set; }
        public bool CurrentUserIsMember { get; set; }
        public int TotalMembersCount { get; set; }
    }
}