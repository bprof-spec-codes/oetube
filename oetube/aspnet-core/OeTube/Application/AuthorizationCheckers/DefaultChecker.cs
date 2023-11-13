using Microsoft.AspNetCore.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace OeTube.Application.AuthorizationCheckers
{
    public class DefaultChecker : AuthorizationChecker,ITransientDependency
    {
        public DefaultChecker(IAuthorizationService authorizationService, ICurrentUser currentUser) : base(authorizationService, currentUser)
        {
        }

        public override Task CheckRightsAsync(object? requestedObject)
        {
            return Task.CompletedTask;
        }
    }
}
