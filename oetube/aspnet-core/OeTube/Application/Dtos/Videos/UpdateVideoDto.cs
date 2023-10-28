using OeTube.Domain.Entities.Videos;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.Videos
{
    public class UpdateVideoMapper : IObjectMapper<UpdateVideoDto, Video>, ITransientDependency
    {
        public Video Map(UpdateVideoDto source)
        {
            throw new NotImplementedException();
        }

        public Video Map(UpdateVideoDto source, Video destination)
        {
            return destination.SetName(source.Name)
                              .SetDescription(source.Description)
                              .SetAccess(source.Access);
        }
    }

    public class UpdateVideoDto
    {
        [StringLength(VideoConstants.NameMaxLength, MinimumLength = VideoConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;

        [StringLength(VideoConstants.NameMaxLength)]
        public string? Description { get; set; }

        public AccessType Access { get; set; } = AccessType.Public;
    }
}