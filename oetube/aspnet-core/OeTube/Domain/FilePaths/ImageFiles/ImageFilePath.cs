
namespace OeTube.Domain.FilePaths.ImageFiles
{

    public abstract class ImageFilePath : FilePath
    {
        public override string Key { get; }

        public ImageFilePath(Guid id)
        {
            Key = id.ToString();
        }
    }
}
