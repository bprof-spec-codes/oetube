using OeTube.Application.Caches;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Repositories;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.Groups
{
    public class CreateUpdateGroupMapper : AsyncObjectMapper<CreateUpdateGroupDto, Group>, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentUser _currentUser;
        private readonly IGroupRepository _groupRepository;
        private readonly IImageUploadHandler _imageUploadHandler;
        private readonly GroupCacheService _cacheService;

        public CreateUpdateGroupMapper(IGuidGenerator guidGenerator,
                                       ICurrentUser currentUser,
                                       IGroupRepository groupRepository,
                                       IImageUploadHandler imageUploadHandler,
                                       GroupCacheService cacheService)
        {
            _guidGenerator = guidGenerator;
            _currentUser = currentUser;
            _groupRepository = groupRepository;
            _imageUploadHandler = imageUploadHandler;
            _cacheService = cacheService;
        }

        public override async Task<Group> MapAsync(CreateUpdateGroupDto source)
        {
            var id = _guidGenerator.Create();
            var group = new Group(id, source.Name, _currentUser.Id);
            return await MapAsync(source, group);
        }

        public override async Task<Group> MapAsync(CreateUpdateGroupDto source, Group destination)
        {
            destination.SetName(source.Name)
                       .SetDescription(source.Description)
                       .UpdateEmailDomains(source.EmailDomains);
            await _groupRepository.UpdateChildrenAsync(destination, source.Members);
            if (source.Image is not null)
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(source.Image);
                await _imageUploadHandler.HandleFileAsync<Group>(new ImageUploadHandlerArgs(destination.Id, content));
            }
            await _cacheService.DeleteMembersCountAsync(destination);
            return destination;
        }
    }

    public class CreateUpdateGroupDto
    {
        [Required]
        [StringLength(GroupConstants.NameMaxLength, MinimumLength = GroupConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;

        [StringLength(GroupConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public List<string> EmailDomains { get; set; } = new List<string>();
        public List<Guid> Members { get; set; } = new List<Guid>();
        public IRemoteStreamContent? Image { get; set; }
    }
}