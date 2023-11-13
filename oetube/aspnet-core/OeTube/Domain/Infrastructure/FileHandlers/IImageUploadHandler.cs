namespace OeTube.Domain.Infrastructure.FileHandlers
{
    public class ImageUploadHandlerArgs
    {
        public ImageUploadHandlerArgs(Guid id, ByteContent content)
        {
            Id = id;
            Content = content;
        }

        public Guid Id { get; }
        public ByteContent Content { get; }
    }

    public interface IImageUploadHandler : IFileHandler<ImageUploadHandlerArgs>
    {
    }
}