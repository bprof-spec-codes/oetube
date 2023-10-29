using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Services.Url
{
    public interface IImageUrlService
    {
        string? GetImageUrl<T>(string methodName, Guid id);
    }

    public class ImageUrlService : IImageUrlService,ITransientDependency
    {
        private readonly IUrlService _urlService;

        public ImageUrlService(IUrlService urlService)
        {
            _urlService = urlService;
        }

        public string? GetImageUrl<T>(string methodName, Guid id)
        {
            return _urlService.GetUrl<T>(methodName, new RouteTemplateParameter(id))
            ?? throw new NullReferenceException(nameof(methodName));
        }
    }
}