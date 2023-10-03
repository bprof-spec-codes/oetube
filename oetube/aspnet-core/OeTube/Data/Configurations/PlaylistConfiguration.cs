using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OeTube.Data.Configurations
{
    public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasKey(p => p.Id);
            builder.OwnsMany(p => p.Items, action =>
            {
                builder.HasOne(typeof(Video)).WithMany().HasForeignKey(nameof(VideoItem.Id));
            })
                .HasOne(typeof(IdentityUser<Guid>)).WithMany().HasForeignKey(nameof(Playlist.CreatorId));
            builder.Property(p => p.Items).HasField("_items");
        }
    }
}
