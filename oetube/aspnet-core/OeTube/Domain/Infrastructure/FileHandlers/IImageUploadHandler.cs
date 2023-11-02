using OeTube.Domain.Configs;
using OeTube.Domain.FilePaths;
using OeTube.Domain.Infrastructure.FileHandlers;

namespace OeTube.Domain.Infrastructure.FileHandlers
{
    public class ImageUploadHandlerArgs
    {
        public ImageUploadHandlerArgs(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public interface IImageUploadHandler: IContentFileHandler<ImageUploadHandlerArgs>
    {

    }
}
