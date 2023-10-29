using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;

namespace OeTube.Domain.Infrastructure.FFmpeg
{
    public interface IProcessUploadTaskFactory
    {
        public ProcessUploadTask Create(Video video, VideoInfo videoInfo);
    }
}