using OeTube.Domain.Infrastructure.FileContainers;

namespace OeTube.Domain.Infrastructure.Images
{
    public class ImageFileClass : FileClass
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
