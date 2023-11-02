using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Services.Url
{
    public interface IImageUrlService
    {
        string GetImageUrl(Guid id);
        string GetThumbnailImageUrl(Guid id);
    }
  
}