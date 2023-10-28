using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;

namespace OeTube.Data.Configurations.Videos
{
    public class VideoConfiguration : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ConfigureByConvention();
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Name)
                   .HasMaxLength(VideoConstants.NameMaxLength)
                   .IsRequired();

            builder.Property(v => v.Description)
                   .HasMaxLength(VideoConstants.DescriptionMaxLength);

            builder.Ignore(v => v.AccessGroups);
            builder.HasMany(v => v.AccessGroups)
                   .WithOne()
                   .HasForeignKey(nameof(AccessGroup.VideoId))
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Ignore(v => v.Resolutions);

            builder.HasMany(v => v.Resolutions)
                   .WithOne()
                   .HasForeignKey(nameof(VideoResolution.VideoId))
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ConfigureCreator<IdentityUser, Video>();
            builder.ConfigureCreationTime();
            builder.HasIndex(v => v.CreationTime).IsUnique(false);
        }
    }
}