using OeTube.Domain.Infrastructure.FileClasses;

namespace OeTube.Infrastructure.FileClasses.VideoFiles
{
    public abstract class VideoFileClass : FileClass,IVideoFileClass
    {
        public const long MaxSourceVideoSize = 1024L * 1024 * 1024;

        public override string Key { get; }
        public VideoFileClass(Guid key)
        {
            Key = key.ToString() ?? string.Empty;
        }
    }
}
