using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.FilePaths.VideoFiles
{
    public class ResizedPath : ResolutionDirectoryPath
    {
        public override string Name => "resized";

        public ResizedPath(Guid key, Resolution resolution) : base(key, resolution)
        {
        }
    }
}