using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace OeTube.Application.Extensions
{
    public static class CurrentUserExtension
    {
        public static void CheckCreator(this ICurrentUser currentUser, Guid? id)
        {
            if (currentUser.Id != id)
            {
                throw new UserFriendlyException("You have no right to modify the content of others.");
            }
        }
        public static void CheckCreator<TCreatedEntity>(this ICurrentUser currentUser, TCreatedEntity entity)
            where TCreatedEntity : class, IEntity, IMayHaveCreator
        {
            currentUser.CheckCreator(entity.CreatorId);
        }
    }
}
