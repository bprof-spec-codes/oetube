using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Services.Url
{
    public class UserUrlService : UrlService, IImageUrlService,ITransientDependency
    {
        public UserUrlService(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public string? GetImageUrl(Guid id)
        {
            return GetUrl<GroupAppService>(nameof(OeTubeUserAppService.GetImageAsync), new RouteTemplateParameter(id));
        }

      
    }

 
}