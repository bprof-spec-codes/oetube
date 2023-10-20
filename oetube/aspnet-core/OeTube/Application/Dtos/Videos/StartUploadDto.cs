using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;
using OeTube.Domain.Entities.Videos;
using FluentValidation;

namespace OeTube.Application.Dtos.Videos
{
    public class StartVideoUploadDto
    {
        [StringLength(VideoConstants.NameMaxLength,MinimumLength =VideoConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;
        [StringLength(VideoConstants.DescriptionMaxLength)]
        public string? Description { get; set; } = string.Empty;
        [Required]
        public IRemoteStreamContent Content { get; set; }
    }

}
