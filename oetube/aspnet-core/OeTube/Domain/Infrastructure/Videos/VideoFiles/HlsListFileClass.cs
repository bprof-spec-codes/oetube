using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Infrastructure.Videos.VideoFiles
{
    public class HlsListFileClass : ResolutionDirectoryFileClass
    {
        public override string Name => "list";

        public HlsListFileClass(Guid key, Resolution resolution) : base(key, resolution)
        {
        }
    }
}
