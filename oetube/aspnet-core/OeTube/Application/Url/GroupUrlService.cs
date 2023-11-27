using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Url
{
    public class GroupUrlService : UrlService, ITransientDependency
    {
        public GroupUrlService(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public string GetImageUrl(Guid id)
        {
            return GetUrl<GroupAppService>(nameof(GroupAppService.GetImageAsync), new RouteTemplateParameter(id));
        }

        public string GetThumbnailImageUrl(Guid id)
        {
            return GetUrl<GroupAppService>(nameof(GroupAppService.GetThumbnailImageAsync), new RouteTemplateParameter(id));
        }
    }
}