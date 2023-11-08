using OeTube.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OeTube.Application.Dtos.OeTubeUsers
{

    public class UpdateUserDto
    {
        [Required]
        [StringLength(OeTubeUserConstants.NameMaxLength, MinimumLength = OeTubeUserConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;

        public string? AboutMe { get; set; }
    }
}