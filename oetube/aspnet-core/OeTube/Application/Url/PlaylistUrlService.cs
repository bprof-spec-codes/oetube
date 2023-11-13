using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Url
{
    public class PlaylistUrlService : UrlService, ITransientDependency
    {
        public PlaylistUrlService(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public string GetImageUrl(Guid id)
        {
            return GetUrl<PlaylistAppService>(nameof(PlaylistAppService.GetImageAsync), new RouteTemplateParameter(id));
        }

        public string GetThumbnailImageUrl(Guid id)
        {
            return GetUrl<PlaylistAppService>(nameof(PlaylistAppService.GetThumbnailImageAsync), new RouteTemplateParameter(id));
        }
    }


}