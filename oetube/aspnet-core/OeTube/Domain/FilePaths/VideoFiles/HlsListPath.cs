using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.FilePaths.VideoFiles
{
    public class HlsListPath : ResolutionDirectoryPath
    {
        public override string Name => "list";

        public HlsListPath(Guid key, Resolution resolution) : base(key, resolution)
        {
        }
    }
}
