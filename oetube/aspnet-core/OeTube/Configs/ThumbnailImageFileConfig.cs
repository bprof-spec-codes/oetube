using OeTube.Domain.Configs;
using Volo.Abp.DependencyInjection;

namespace OeTube.Configs
{
 
    public class ThumbnailImageFileConfig : IThumbnailImageFileConfig,ISingletonDependency
    {
        public ThumbnailImageFileConfig()
        {
            MaxWidth = 300;
            MaxHeight =300;
            OutputFormat = "jpg";
        }

        public int MaxWidth { get; }
        public int MaxHeight { get; }
        public string OutputFormat { get; }


    }
}