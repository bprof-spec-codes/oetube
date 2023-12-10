using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OeTube.Domain.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;

namespace OeTube.Data.Configurations
{
    public class OeTubeUserConfiguration : IEntityTypeConfiguration<OeTubeUser>
    {
        public void Configure(EntityTypeBuilder<OeTubeUser> builder)
        {
            builder.ConfigureByConvention();
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                   .HasMaxLength(OeTubeUserConstants.NameMaxLength)
                   .IsRequired();
            builder.Property(u => u.AboutMe)
                   .HasMaxLength(OeTubeUserConstants.AboutMeMaxLength);

            builder.HasIndex(u => u.CreationTime).IsUnique(false);
            builder.HasIndex(u => u.EmailDomain).IsUnique(false);
            builder.HasOne(typeof(IdentityUser))
                   .WithOne()
                   .HasForeignKey(typeof(OeTubeUser), nameof(OeTubeUser.Id))
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}