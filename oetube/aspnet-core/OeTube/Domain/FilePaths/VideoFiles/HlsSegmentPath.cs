using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.FilePaths.VideoFiles
{
    public class HlsSegmentPath : ResolutionDirectoryPath
    {
        public override string Name { get; }
        public HlsSegmentPath(Guid key, Resolution resolution, int segment) : base(key, resolution)
        {
            Name = segment.ToString();
        }

    }
}
