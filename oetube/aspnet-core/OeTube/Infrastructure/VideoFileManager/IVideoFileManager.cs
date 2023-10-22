using OeTube.Domain.Entities.Videos;
using OeTube.Infrastructure.FF;
using OeTube.Infrastructure.FF.Probe.Infos;

namespace OeTube.Infrastructure.VideoFileManager
{
    public interface IVideoFileManager
    {
        Resolution DefaultResolution { get; }
        long MaxSizeInBytes { get; }
        string OutputFormat { get; }

        List<UploadTask> CreateUploadTasks(Video video);
        IEnumerable<Resolution> GetDesiredResolutions(Resolution originalResolution);
        IEnumerable<Resolution> GetPossibleResolutions();
        IEnumerable<string> GetSupportedCodecs();
        IEnumerable<string> GetSupportedFormats();
        bool IsInDesiredResolutionAndFormat(VideoInfo videoInfo);
        void ValidateResizedVideo(Video video, VideoInfo sourceInfo, VideoInfo resizedInfo);
        void ValidateSourceVideo(VideoInfo sourceInfo);
    }
}