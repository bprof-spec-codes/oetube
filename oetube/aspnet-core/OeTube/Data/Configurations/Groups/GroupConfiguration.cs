using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OeTube.Domain.Entities.Groups;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;

namespace OeTube.Data.Configurations.Groups
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ConfigureByConvention();
            builder.HasKey(g => g.Id);

            builder.Property(p => p.Name)
                   .HasMaxLength(GroupConstants.NameMaxLength);
            builder.Property(p => p.Description)
                    .HasMaxLength(GroupConstants.DescriptionMaxLength);

            builder.Ignore(g => g.Members);
            builder.HasMany(g => g.Members)
                   .WithOne()
                   .HasForeignKey(nameof(Member.GroupId))
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ConfigureCreator<IdentityUser, Group>();
            builder.ConfigureCreationTimeIndex();

            builder.Ignore(g => g.EmailDomains);
            builder.HasMany(g => g.EmailDomains)
                   .WithOne()
                   .HasForeignKey(nameof(EmailDomain.GroupId))
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}