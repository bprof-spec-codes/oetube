using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OeTube.Domain.Entities.Groups;

namespace OeTube.Data.Configurations.Groups
{
    public class EmailDomainConfiguration : IEntityTypeConfiguration<EmailDomain>
    {
        public void Configure(EntityTypeBuilder<EmailDomain> builder)
        {
            builder.HasKey(nameof(EmailDomain.GroupId), nameof(EmailDomain.Domain));
            builder.Property(o => o.Domain)
                   .HasMaxLength(EmailDomainConstants.DomainMaxLength);
        }
    }
}
