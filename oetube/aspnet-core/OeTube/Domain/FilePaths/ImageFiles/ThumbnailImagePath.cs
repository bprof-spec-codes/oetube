
namespace OeTube.Domain.FilePaths.ImageFiles
{
    public class ThumbnailImagePath : ImageFilePath,IDefaultFilePath
    {
        public ThumbnailImagePath(Guid id) : base(id)
        {
        }

   

        public override string Name => "thumbnail_image";

        public static string GetDefaultPath(string? format = null)
        {
            return "default_thumbnail_image." + format;
        }
    }
}
