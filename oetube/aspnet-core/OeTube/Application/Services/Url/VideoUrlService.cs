using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Services.Url
{


    public interface IVideoUrlService:IUrlService
    {
        string GetHlsListUrl(Guid id, int width, int height);
        string GetHlsSegmentUrl(Guid id, int width, int height, int segment);
        string GetIndexImageByIndexUrl(Guid id, int index);
        string GetIndexImageUrl(Guid id);

    }
    public class VideoUrlService : UrlService, IVideoUrlService, ITransientDependency
    {
        public VideoUrlService(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public string GetHlsListUrl(Guid id, int width, int height)
        {
            return GetUrl<VideoAppService>(nameof(VideoAppService.GetHlsListAsync),
                                              new(id),
                                              new(width),
                                              new(height));
        }

        public string GetHlsSegmentUrl(Guid id, int width, int height, int segment)
        {
            return GetUrl<VideoAppService>(nameof(VideoAppService.GetHlsSegmentAsync),
                                            new(id),
                                            new(width),
                                            new(height),
                                            new(segment));
        }

        public string GetIndexImageUrl(Guid id)
        {
            return GetUrl<VideoAppService>(nameof(VideoAppService.GetIndexImageAsync), new RouteTemplateParameter(id));
        }
        public string GetIndexImageByIndexUrl(Guid id, int index)
        {
            return GetUrl<VideoAppService>(nameof(VideoAppService.GetIndexImageByIndexAsync), new(id),new(index));
        }
    }
}