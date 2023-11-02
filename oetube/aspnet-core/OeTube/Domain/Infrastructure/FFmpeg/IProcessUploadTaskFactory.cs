using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;
using OeTube.Domain.Infrastructure.FileHandlers;

namespace OeTube.Domain.Infrastructure.FFmpeg
{
    public interface IProcessUploadTaskFactory
    {
        public ProcessVideoUploadArgs Create(Video video, VideoInfo videoInfo);
    }
}