using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OeTube.Data.Configurations
{
    public class VideoConfiguration : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.HasKey(p => p.Id);
            builder.OwnsMany(p => p.AccessGroups, action =>
            {
                builder.HasOne(typeof(Group)).WithMany().HasForeignKey(nameof(AccessGroup.Id));
            })
                .HasOne(typeof(IdentityUser<Guid>)).WithMany().HasForeignKey(nameof(Video.CreatorId));
            builder.Property(p => p.AccessGroups).HasField("_accessGroups");
        }
    }
}
