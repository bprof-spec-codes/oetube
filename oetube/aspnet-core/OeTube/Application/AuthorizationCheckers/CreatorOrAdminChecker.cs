using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace OeTube.Application.AuthorizationCheckers
{
    public class CreatorOrAdminChecker: CreatorChecker, ITransientDependency
    {
        public CreatorOrAdminChecker(IAuthorizationService authorizationService, ICurrentUser currentUser) : base(authorizationService, currentUser)
        {
        }

        public override Task CheckRightsAsync(object? requestedObject)
        {
            if (CurrentUser.Roles.Contains("admin")) {
                return Task.CompletedTask;
            }
            return base.CheckRightsAsync(requestedObject);
        }
    }
}