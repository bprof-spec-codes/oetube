using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Infrastructure.Videos
{
    public class VideoFileConfig
    {
        public VideoFileConfig(long maxSizeInBytes,
                               string outputFormat,
                               IReadOnlyList<string> supportedCodecs,
                               IReadOnlyList<string> supportedFormats,
                               IReadOnlyList<Resolution> resolutions)
        {
            MaxSizeInBytes = maxSizeInBytes;
            OutputFormat = outputFormat;
            SupportedCodecs = supportedCodecs;
            SupportedFormats = supportedFormats;
            Resolutions = resolutions;
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