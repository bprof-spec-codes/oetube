using OeTube.Domain.Configs;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Domain.Infrastructure.Videos
{
    public class VideoFileConfig : IVideoFileConfig,ISingletonDependency
    {
        public VideoFileConfig()
        {
            MaxSizeInBytes= 1024L * 1024 * 1024;
            OutputFormat = "mp4";
            SupportedFormats = new string[] { "mp4" };
            SupportedCodecs = Array.Empty<string>();
            Resolutions = new Resolution[] { Resolution.SD, Resolution.HD, Resolution.FHD };
        }

        public long MaxSizeInBytes { get; }
        public string OutputFormat { get; }
        public IReadOnlyList<string> SupportedCodecs { get; }
        public IReadOnlyList<string> SupportedFormats { get; }
        public IReadOnlyList<Resolution> Resolutions { get; }

        public IEnumerable<Resolution> GetDesiredResolutions(Resolution originalResolution)
        {
            yield return Resolutions[0];
            for (int i = 1; i < Resolutions.Count; i++)
            {
                if (originalResolution.Height < Resolutions[i].Height)
                {
                    break;
                }
                yield return Resolutions[i];
            }
        }
    }
}