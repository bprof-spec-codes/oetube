using AutoMapper.Internal.Mappers;
using JetBrains.Annotations;
using OeTube.Domain.Entities.Groups;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.Groups
{
    public class CreateUpdateGroupMapper : IObjectMapper<CreateUpdateGroupDto, Group>,ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentUser _currentUser;
        public CreateUpdateGroupMapper(IGuidGenerator guidGenerator,ICurrentUser currentUser)
        {
            _guidGenerator = guidGenerator;
            _currentUser = currentUser;
        }

        public Group Map(CreateUpdateGroupDto source)
        {
            if(_currentUser.Id is null)
            {
                throw new NullReferenceException(nameof(_currentUser.Id));
            }
            var group = new Group(_guidGenerator.Create(), source.Name, _currentUser.Id.Value)
                            .SetDescription(source.Description);

            return group;
        }

        public Group Map(CreateUpdateGroupDto source, Group destination)
        {
            return destination.SetName(source.Name)
                               .SetDescription(source.Description);
        }
    }
    public class CreateUpdateGroupDto
    {
        [Required]
        [StringLength(GroupConstants.NameMaxLength, MinimumLength = GroupConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;
        [StringLength(GroupConstants.NameMaxLength)]
        public string? Description { get; set; }
    }
}
