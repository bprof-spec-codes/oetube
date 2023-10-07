using JetBrains.Annotations;
using OeTube.Domain.Entities.Groups;
using System.ComponentModel.DataAnnotations;

namespace OeTube.Application.Dtos.Groups
{
    public class CreateUpdateGroupDto
    {
        [Required]
        [StringLength(GroupConstants.NameMaxLength, MinimumLength = GroupConstants.NameMinLength)]
        public string Name { get; set; }
        [StringLength(GroupConstants.NameMaxLength)]
        public string? Description { get; set; }
    }
}
