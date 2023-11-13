using Microsoft.AspNetCore.Authorization;
using OeTube.Domain.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace OeTube.Application.AuthorizationCheckers
{
    public class OwnerChecker : AuthorizationChecker, ITransientDependency
    {
        public OwnerChecker(IAuthorizationService authorizationService, ICurrentUser currentUser) : base(authorizationService, currentUser)
        {
        }

        public override Task CheckRightsAsync(object? requestedObject)
        {
            if(requestedObject is OeTubeUser user&&user.Id!=CurrentUser.Id)
            {
                throw new InvalidOperationException();
            }
            return Task.CompletedTask;
        }
    }
}
