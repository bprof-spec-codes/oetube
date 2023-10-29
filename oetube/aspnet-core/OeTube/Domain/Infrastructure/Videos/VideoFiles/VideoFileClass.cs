using OeTube.Domain.Infrastructure.FileContainers;

namespace OeTube.Domain.Infrastructure.Videos.VideoFiles
{
    public abstract class VideoFileClass : FileClass
    {
        public const long MaxSourceVideoSize = 1024L * 1024 * 1024;

        public override string Key { get; }
        public VideoFileClass(Guid key)
        {
            Key = key.ToString() ?? string.Empty;
        }
    }
}
