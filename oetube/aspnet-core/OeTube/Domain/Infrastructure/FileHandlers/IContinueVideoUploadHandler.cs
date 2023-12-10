using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Infrastructure.FileHandlers
{
    public class ContinueVideoUploadHandlerArgs
    {
        public Video Video { get; }
        public ByteContent Content { get; }

        public ContinueVideoUploadHandlerArgs(Video video, ByteContent content)
        {
            Video = video;
            Content = content;
        }
    }

    public interface IContinueVideoUploadHandler : IFileHandler<ContinueVideoUploadHandlerArgs, Video>
    { }
}