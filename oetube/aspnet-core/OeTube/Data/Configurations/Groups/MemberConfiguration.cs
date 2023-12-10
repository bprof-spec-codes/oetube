using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OeTube.Domain.Entities.Groups;
using Volo.Abp.Identity;

namespace OeTube.Data.Configurations.Groups
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(nameof(Member.GroupId), nameof(Member.UserId));
            builder.HasOne(typeof(IdentityUser))
                   .WithMany()
                   .HasForeignKey(nameof(Member.UserId))
                   .OnDelete(DeleteBehavior.Cascade);
            builder.ConfigureCreationTimeIndex();
        }
    }
}