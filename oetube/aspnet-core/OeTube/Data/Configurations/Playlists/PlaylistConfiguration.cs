using Autofac.Features.OwnedInstances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OeTube.Domain.Entities.Playlists;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;

namespace OeTube.Data.Configurations.Playlists
{

    public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.ConfigureByConvention();
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .HasMaxLength(PlaylistConstants.NameMinLength)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(PlaylistConstants.DescriptionMaxLength);

            builder.Ignore(p => p.Items);
            builder.HasMany(p => p.Items)
                   .WithOne()
                   .HasForeignKey(nameof(VideoItem.PlaylistId))
                   .OnDelete(DeleteBehavior.Cascade);
            builder.ConfigureCreator<IdentityUser, Playlist>();
            builder.ConfigureCreationTimeIndex();
        }
    }
}
