using OeTube.Domain.Entities.Videos;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;

namespace OeTube.Application.Dtos.Videos
{
    public class StartVideoUploadDto
    {
        [StringLength(VideoConstants.NameMaxLength, MinimumLength = VideoConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;

        [StringLength(VideoConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public AccessType Access { get; set; } = AccessType.Public;

        [Required]
        public IRemoteStreamContent? Content { get; set; }
    }
}