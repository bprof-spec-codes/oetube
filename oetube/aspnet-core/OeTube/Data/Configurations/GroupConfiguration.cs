using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OeTube.Data.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(p => p.Id);
            builder.OwnsMany(p => p.Members, action =>
            {
                builder.HasOne(typeof(IdentityUser<Guid>)).WithMany().HasForeignKey(nameof(Member.Id));
            })
                .HasOne(typeof(IdentityUser<Guid>)).WithMany().HasForeignKey(nameof(Group.CreatorId));
            builder.Property(p => p.Members).HasField("_members");
        }
    }
}
