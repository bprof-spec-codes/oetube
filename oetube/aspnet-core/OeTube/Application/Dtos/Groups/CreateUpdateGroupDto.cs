using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.Groups
{
    public class CreateUpdateGroupMapper : IObjectMapper<CreateUpdateGroupDto, Group>, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentUser _currentUser;
        private readonly IGroupRepository _groupRepository;
        public CreateUpdateGroupMapper(IGuidGenerator guidGenerator, ICurrentUser currentUser, IGroupRepository groupRepository)
        {
            _guidGenerator = guidGenerator;
            _currentUser = currentUser;
            _groupRepository = groupRepository;
        }

        public Group Map(CreateUpdateGroupDto source)
        {
            var id = _guidGenerator.Create();
            var group = new Group(id, source.Name, _currentUser.Id);
            return Map(source,group);
        }

        public Group Map(CreateUpdateGroupDto source, Group destination)
        {
                    destination.SetName(source.Name)
                               .SetDescription(source.Description)
                               .UpdateEmailDomains(source.EmailDomains);
            _groupRepository.UpdateChildEntitiesAsync(destination, source.Members, false);
            return destination;
        }
    }

    public class CreateUpdateGroupDto
    {
        [Required]
        [StringLength(GroupConstants.NameMaxLength, MinimumLength = GroupConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;

        [StringLength(GroupConstants.NameMaxLength)]
        public string? Description { get; set; }

        public List<string> EmailDomains { get; set; } = new List<string>();
        public List<Guid> Members { get; set; } = new List<Guid>();
    }
}