using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Services.Url
{
    public interface IVideoUrlService
    {
        string GetHlsListUrl(Guid id, int width, int height);

        string GetHlsSegmentUrl(Guid id, int width, int height, int segment);

        string GetIndexImageUrl(Guid id);
    }

    public class VideoUrlService : IVideoUrlService, ITransientDependency
    {
        private readonly IUrlService _urlService;

        public VideoUrlService(IUrlService urlService)
        {
            _urlService = urlService;
        }

        public string GetHlsListUrl(Guid id, int width, int height)
        {
            return _urlService.GetUrl<VideoAppService>(nameof(VideoAppService.GetHlsListAsync),
                                              new(id),
                                              new(width),
                                              new(height)) ??
              throw new NullReferenceException(nameof(VideoAppService.GetHlsListAsync));
        }

        public string GetHlsSegmentUrl(Guid id, int width, int height, int segment)
        {
            return _urlService.GetUrl<VideoAppService>(nameof(VideoAppService.GetHlsSegmentAsync),
                                            new(id),
                                            new(width),
                                            new(height),
                                            new(segment)) ??
            throw new NullReferenceException(nameof(VideoAppService.GetHlsSegmentAsync));
        }

        public string GetIndexImageUrl(Guid id)
        {
            return _urlService.GetUrl<VideoAppService>(nameof(VideoAppService.GetIndexImageAsync), new RouteTemplateParameter(id))
              ?? throw new NullReferenceException(nameof(VideoAppService.GetIndexImageAsync));
        }
    }
}