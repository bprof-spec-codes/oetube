using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Users;

namespace OeTube.Application.AuthorizationCheckers
{
    public interface IAuthorizationChecker
    {
        string? PolicyName { get; set; }
        public ICurrentUser CurrentUser { get; }

        Task CheckPolicyAsync();

        Task CheckRightsAsync(object? requestedObject);
    }

    public interface IAuthorizationManyChecker<T> : IAuthorizationChecker
    {
        Task CheckRightsManyAsync(IEnumerable<T> requestedObjects);
    }

    public abstract class AuthorizationChecker : IAuthorizationChecker
    {
        public virtual string? PolicyName { get; set; }
        protected IAuthorizationService AuthorizationService { get; }
        public ICurrentUser CurrentUser { get; }

        protected AuthorizationChecker(IAuthorizationService authorizationService, ICurrentUser currentUser)
        {
            AuthorizationService = authorizationService;
            CurrentUser = currentUser;
        }

        public async Task CheckPolicyAsync()
        {
            if (!string.IsNullOrWhiteSpace(PolicyName))
            {
                await AuthorizationService.CheckAsync(PolicyName);
            }
        }

        public abstract Task CheckRightsAsync(object? requestedObject);
    }
}