using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OeTube.Data.Configurations.Videos
{
    public class AccessGroupConfiguration : IEntityTypeConfiguration<AccessGroup>
    {
        public void Configure(EntityTypeBuilder<AccessGroup> builder)
        {
            builder.HasKey(nameof(AccessGroup.VideoId), nameof(AccessGroup.GroupId));
            builder.HasOne(typeof(Group))
                   .WithMany()
                   .HasForeignKey(nameof(AccessGroup.GroupId))
                   .OnDelete(DeleteBehavior.Cascade);
            builder.ConfigureCreationTime();
        }
    }
}
