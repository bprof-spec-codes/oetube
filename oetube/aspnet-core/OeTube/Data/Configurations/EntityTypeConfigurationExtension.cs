using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Data.Configurations
{
    public static class EntityTypeConfigurationExtension
    {
        public static void ConfigureCreator<TCreator, TCreation>(this EntityTypeBuilder<TCreation> builder)
         where TCreator:IAggregateRoot<Guid>
         where TCreation:class,IMayHaveCreator
        {
            builder.HasOne(typeof(TCreator))
                .WithMany()
                .HasForeignKey(nameof(IMayHaveCreator.CreatorId));
        } 
        public static void ConfigureCreationTimeIndex<TCreation>(this EntityTypeBuilder<TCreation> builder)
            where TCreation:class,IHasCreationTime
        {
            builder.HasIndex(c => c.CreationTime).IsUnique(false);
        }
    }
}
