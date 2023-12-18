using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg;

namespace OeTube.Domain.Infrastructure.FileHandlers
{
    public class VideoResizingArgs
    {
        public Guid Id { get; set; }
        public List<UploadTask> Tasks { get; set; } = new List<UploadTask>();
    }
    public interface IVideoResizingHandler: IFileHandler<VideoResizingArgs>
    {
    }
}