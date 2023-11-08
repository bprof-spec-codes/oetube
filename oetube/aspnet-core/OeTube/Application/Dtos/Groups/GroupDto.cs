using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Services.Caches.GroupCache;
using OeTube.Application.Services.Url;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.Groups
{
    public class GroupMapper : IObjectMapper<Group, GroupDto>, ITransientDependency
    {
        private readonly IObjectMapper<Guid?, CreatorDto?> _creatorMapper;
        private readonly IImageUrlService _urlService;
        private readonly ICurrentUser _currentUser;
        private readonly IIsMemberCacheService _isMemberCache;
        private readonly IGroupMembersCountCacheService _groupMembersCountCache;
        public GroupMapper(GroupUrlService urlService, IObjectMapper<Guid?, CreatorDto?> creatorMapper, ICurrentUser currentUser, IDistributedCache<GroupMembersCountCacheKey, GroupMembersCountCacheItem> countCache, IGroupMembersCountCacheService groupMembersCountCache, IIsMemberCacheService isMemberCache)
        {
            _urlService = urlService;
            _creatorMapper = creatorMapper;
            _currentUser = currentUser;
            _groupMembersCountCache = groupMembersCountCache;
            _isMemberCache = isMemberCache;
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
            destination.Creator = _creatorMapper.Map(source.CreatorId);
            destination.CurrentUserIsMember = _isMemberCache.IsMemberAsync(_currentUser.Id, source).Result;
            destination.TotalMembersCount = _groupMembersCountCache.GetTotalCountAsync(source).Result;

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
        public bool CurrentUserIsMember { get; set; }
        public int TotalMembersCount { get; set; }
    }
}