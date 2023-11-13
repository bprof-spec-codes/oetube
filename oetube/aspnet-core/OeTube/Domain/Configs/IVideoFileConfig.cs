using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Configs
{
    public interface IVideoFileConfig
    {
        long MaxSizeInBytes { get; }
        string OutputFormat { get; }
        IReadOnlyList<Resolution> Resolutions { get; }
        IReadOnlyList<string> SupportedCodecs { get; }
        IReadOnlyList<string> SupportedFormats { get; }

        IEnumerable<Resolution> GetDesiredResolutions(Resolution originalResolution);
    }
}