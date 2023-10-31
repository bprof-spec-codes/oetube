using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Infrastructure.FFmpeg
{
    public interface IUploadTaskFactory
    {
        public UploadTask Create(Resolution resolution);
    }
}