using OeTube.Domain.Entities.Groups;
using OeTube.Entities;
using System.ComponentModel.DataAnnotations;

namespace OeTube.Application.Dtos.OeTubeUsers
{

    public class UpdateOeTubeUserDto
    {
        [Required]
        [StringLength(OeTubeUserConstants.NameMaxLength, MinimumLength = OeTubeUserConstants.NameMinLength)]
        public string Name { get; set; }
        public string? AboutMe { get; set; }
    }
}
