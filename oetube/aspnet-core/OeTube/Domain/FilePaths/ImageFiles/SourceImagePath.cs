namespace OeTube.Domain.FilePaths.ImageFiles
{
    public class SourceImagePath : ImageFilePath, IDefaultFilePath
    {
        public SourceImagePath(Guid id) : base(id)
        {
        }

        public static string GetDefaultPath(string? format = null)
        {
            return "default_image." + format;
        }

        public override string Name => "image";
    }
}