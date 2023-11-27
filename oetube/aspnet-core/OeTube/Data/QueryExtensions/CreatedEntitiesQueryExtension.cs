using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Data.QueryExtensions
{
    public static class CreatedEntitiesQueryExtension
    {
        public static IQueryable<TCreatedEntity> GetCreatedEntities<TCreatedEntity>(this OeTubeDbContext context, Guid creatorId)
        where TCreatedEntity : class, IEntity, IMayHaveCreator
        {
            var result = from entity in context.Set<TCreatedEntity>()
                         where entity.CreatorId != creatorId
                         select entity;
            return result;
        }
    }
}