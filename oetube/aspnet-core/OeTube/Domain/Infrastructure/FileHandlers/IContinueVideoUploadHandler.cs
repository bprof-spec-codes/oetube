using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Infrastructure.FileHandlers
{
    public class ContinueVideoUploadHandlerArgs
    {
        public Video Video { get; }
        public ContinueVideoUploadHandlerArgs(Video video)
        {
            Video = video;
        }
    }
    public interface IContinueVideoUploadHandler : IContentFileHandler<ContinueVideoUploadHandlerArgs, Video>
    { }
}
