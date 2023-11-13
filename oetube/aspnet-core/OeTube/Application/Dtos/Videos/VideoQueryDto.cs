using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.QueryArgs;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.Videos
{
    public class VideoQueryDto :QueryDto, IVideoQueryArgs
    {
        [StringLength(VideoConstants.NameMaxLength)]
        public string? Name { get; set; }
        public DateTime? CreationTimeMin { get; set; }
        public DateTime? CreationTimeMax { get; set; }
        public TimeSpan? DurationMin { get; set; }
        public TimeSpan? DurationMax { get; set; }
        public Guid? CreatorId { get; set; }
    }
}