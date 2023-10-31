using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;

namespace OeTube.Data.Configurations.Playlists
{
    public class VideoItemConfiguration : IEntityTypeConfiguration<VideoItem>
    {
        public void Configure(EntityTypeBuilder<VideoItem> builder)
        {
            builder.HasKey(nameof(VideoItem.PlaylistId), nameof(VideoItem.Order));
            builder.HasOne(typeof(Video))
                   .WithMany()
                   .HasForeignKey(nameof(VideoItem.VideoId))
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}