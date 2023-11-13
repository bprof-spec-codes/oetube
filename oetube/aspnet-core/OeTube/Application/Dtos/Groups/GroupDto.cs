using NUglify.JavaScript.Syntax;
using OeTube.Application.Caches;
using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Url;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using System.Linq.Expressions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.Groups
{

    public class GroupMapper : AsyncNewDestinationObjectMapper<Group, GroupDto>, ITransientDependency
    {
        private readonly GroupUrlService _urlService;
        private readonly CreatorDtoMapper _creatorMapper;
        private readonly GroupCacheService _cacheService;
        public GroupMapper(GroupUrlService urlService, CreatorDtoMapper creatorMapper,GroupCacheService cacheService,IGroupRepository repository)
        {
            _urlService = urlService;
            _creatorMapper = creatorMapper;
            _cacheService = cacheService;
            _cacheService.ConfigureMembersCount(repository)
                         .ConfigureCurrentUserIsMember(repository);
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

    public static class GroupCacheExtension
    {
        public static GroupCacheService ConfigureMembersCount(this GroupCacheService cacheService, IGroupRepository repository)
        {
            cacheService.GlobalDtoCache.ConfigureProperty<GroupDto, int>(g => g.TotalMembersCount, async (key, group, userId) => await repository.GetMembersCountAsync(group!), TimeSpan.FromSeconds(30));
            return cacheService;
        }
        public static async Task<int> GetOrAddMembersCountAsync(this GroupCacheService cacheService,Group group)
        {
            return await cacheService.GlobalDtoCache.GetOrAddAsync<GroupDto, int>(group, g => g.TotalMembersCount);
        }
        public static async Task DeleteMembersCountAsync(this GroupCacheService cacheService,Group group)
        {
            await cacheService.GlobalDtoCache.DeleteAsync<GroupDto,int>(group, g => g.TotalMembersCount);
        }
        public static async Task<bool> GetOrAddCurrentUserIsMemberAsync(this GroupCacheService cacheService,Group group)
        {
            return await cacheService.RequesterDtoCache.GetOrAddAsync<GroupDto, bool>(group, g => g.CurrentUserIsMember);
        }
      
        public static GroupCacheService ConfigureCurrentUserIsMember(this GroupCacheService cacheService, IGroupRepository repository)
        {
            cacheService.RequesterDtoCache.ConfigureProperty<GroupDto,bool>(g=>g.CurrentUserIsMember, async (key, group, userId) => await repository.IsMemberAsync(userId, group!), TimeSpan.FromSeconds(30));
            return cacheService;
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