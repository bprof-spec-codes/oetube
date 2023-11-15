using OeTube.Application.AuthorizationCheckers;
using OeTube.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Methods
{
    public abstract class ApplicationMethod
    {
        protected IAbpLazyServiceProvider ServiceProvider { get; }
        public IAuthorizationChecker? Authorization { get; set; }
        protected ApplicationMethod(IAbpLazyServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void SetAuthorization(Type authorizationCheckerType)
        {
            Authorization = ServiceProvider
                .LazyGetRequiredService(authorizationCheckerType) as IAuthorizationChecker;
        }

        protected async Task CheckPolicyAsync()
        {
            if (Authorization is not null)
            {
                await Authorization.CheckPolicyAsync();
            }
        }
        protected ILocalEventBus GetLocalEventBus()
        {
            return ServiceProvider.LazyGetRequiredService<ILocalEventBus>();
        }
        protected async Task CheckRightsAsync(object requestedObject)
        {
            if (Authorization is not null)
            {
                await Authorization.CheckRightsAsync(requestedObject);
            }
        }

        protected virtual async Task<TDestination> MapAsync<TSource, TDestination>(TSource source, TDestination destination)
        {
            var mapper = ServiceProvider.LazyGetRequiredService<IObjectMapper<TSource, TDestination>>();
            if (mapper is IAsyncObjectMapper<TSource, TDestination> asyncMapper)
            {
                return await asyncMapper.MapAsync(source, destination);
            }
            else
            {
                return await Task.FromResult(mapper.Map(source, destination));
            }
        }

        protected virtual async Task<TDestination> MapAsync<TSource, TDestination>(TSource source)
        {
            var mapper = ServiceProvider.LazyGetRequiredService<IObjectMapper<TSource, TDestination>>();
            if (mapper is IAsyncObjectMapper<TSource, TDestination> asyncMapper)
            {
                return await asyncMapper.MapAsync(source);
            }
            else
            {
                return await Task.FromResult(mapper.Map(source));
            }
        }
    }
}