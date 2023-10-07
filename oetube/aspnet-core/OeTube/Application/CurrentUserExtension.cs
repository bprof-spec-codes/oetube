﻿using Volo.Abp.Auditing;
using Volo.Abp.Users;

namespace OeTube.Application
{
    public static class CurrentUserExtension
    {
        public static bool IsOwner<TCreated>(this ICurrentUser user, TCreated created)
            where TCreated : IMayHaveCreator
        {
            return created.CreatorId != null && created.CreatorId == user.Id;
        }
        public static void CheckIsOwner<TCreated>(this ICurrentUser user, TCreated created)
            where TCreated : IMayHaveCreator
        {
            if (!user.IsOwner(created))
            {
                throw new ArgumentException();
            }
        }
    }
}
