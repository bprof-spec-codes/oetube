using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OeTube.Data.Configurations.Videos
{

    public class VideoResolutionConfiguration:IEntityTypeConfiguration<VideoResolution>
    {
        public void Configure(EntityTypeBuilder<VideoResolution> builder)
        {
            builder.ConfigureByConvention();
            builder.HasKey(nameof(VideoResolution.VideoId),nameof(VideoResolution.Width),nameof(VideoResolution.Height));
        }
    }
}
