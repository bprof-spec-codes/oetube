using OeTube.Domain.Entities.Videos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Domain.Infrastructure.Videos
{
    public interface IVideoFileConfigFactory
    {
        VideoFileConfig Create();
    }

    public class VideoFileConfigFactory : IVideoFileConfigFactory, ITransientDependency
    {
        public VideoFileConfig Create()
        {
            long maxSizeInBytes = 1024L * 1024 * 1024;
            string outputFormat = "mp4";
            IReadOnlyList<string> supportedFormats = new string[] { "mp4" };
            IReadOnlyList<string> supportedCodecs = Array.Empty<string>();
            IReadOnlyList<Resolution> resolutions = new Resolution[] { Resolution.SD, Resolution.HD, Resolution.FHD };
            return new VideoFileConfig(maxSizeInBytes, outputFormat, supportedFormats, supportedCodecs, resolutions);
        }
    }
}