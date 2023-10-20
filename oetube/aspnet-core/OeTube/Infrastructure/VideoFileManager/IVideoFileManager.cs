using OeTube.Domain.Entities.Videos;
using OeTube.Infrastructure.FFprobe.Infos;

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
        bool IsInDesiredResolutionAndFormat(Video video, VideoStreamInfo videoStreamInfo);
        void ValidateResizedVideo(Video video, VideoInfo sourceInfo, VideoInfo resizedInfo);
        void ValidateSourceVideo(VideoInfo sourceInfo);
    }
}