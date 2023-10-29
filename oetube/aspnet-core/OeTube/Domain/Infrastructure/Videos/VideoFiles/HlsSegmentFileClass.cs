using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Infrastructure.Videos.VideoFiles
{
    public class HlsSegmentFileClass : ResolutionDirectoryFileClass
    {
        public override string Name { get; }
        public HlsSegmentFileClass(Guid key, Resolution resolution, int segment) : base(key, resolution)
        {
            Name = segment.ToString();
        }

    }
}
