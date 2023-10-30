using OeTube.Domain.Infrastructure.FileClasses;
using OeTube.Domain.Infrastructure.Videos;

namespace OeTube.Infrastructure.FileClasses
{
    public class ImageFileClass : FileClass,IGroupFileClass,IUserFileClass,IPlaylistFileClass
    {
        public override string Key { get; }
        public override string Name => "image";
        public override string MimeTypeCategory => "image";

        public ImageFileClass(Guid id)
        {
            Key = id.ToString();
        }
    }
}
