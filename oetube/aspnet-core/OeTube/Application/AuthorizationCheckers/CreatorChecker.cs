using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace OeTube.Application.AuthorizationCheckers
{
    public class CreatorChecker : AuthorizationChecker,ITransientDependency
    {
        public CreatorChecker(IAuthorizationService authorizationService, ICurrentUser currentUser) : base(authorizationService, currentUser)
        {
        }

        public override Task CheckRightsAsync(object? requestedObject)
        {
            if(requestedObject is IMayHaveCreator mayHave&& 
               mayHave.CreatorId is not null&&
               mayHave.CreatorId != CurrentUser.Id)
            {
                throw new InvalidOperationException();
            }
            if(requestedObject is IMustHaveCreator mustHave&&
                mustHave.CreatorId != CurrentUser.Id)
            {
                throw new InvalidOperationException();
            }
            return Task.CompletedTask;
        }

    }
}
