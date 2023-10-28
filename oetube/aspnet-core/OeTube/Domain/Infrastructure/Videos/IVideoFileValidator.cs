using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;

namespace OeTube.Domain.Infrastructure.Videos
{
    public interface IVideoFileValidator
    {
        bool IsInDesiredResolutionAndFormat(VideoInfo videoInfo);

        void ValidateResizedVideo(Video video, VideoInfo sourceInfo, VideoInfo resizedInfo);

        void ValidateSourceVideo(VideoInfo sourceInfo);
    }
}