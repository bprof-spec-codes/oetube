using OeTube.Domain.Configs;
using Volo.Abp.DependencyInjection;

namespace OeTube.Configs
{
    public class SourceImageFileConfig : ISourceImageFileConfig,ISingletonDependency
    {
        public SourceImageFileConfig()
        {
            MaxWidth = 1000;
            MaxHeight = 1000;
            OutputFormat = "jpg";
        }

        public int MaxWidth { get; }
        public int MaxHeight { get; }
        public string OutputFormat { get; }
    }
}