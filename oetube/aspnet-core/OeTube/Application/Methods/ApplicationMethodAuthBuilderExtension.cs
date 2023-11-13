namespace OeTube.Application.Methods
{
    public static class ApplicationMethodAuthBuilderExtension
    {
        public static TAppMethod SetAuthorizationAndPolicy<TAppMethod>(this TAppMethod appMethod, Type authorizationCheckerType, string? policyName = null)
        where TAppMethod : ApplicationMethod
        {
            appMethod.SetAuthorization(authorizationCheckerType);
            if (appMethod.Authorization is not null)
            {
                appMethod.Authorization.PolicyName = policyName;
            }
            return appMethod;
        }
    }
}