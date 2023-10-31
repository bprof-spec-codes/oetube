using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.FileClasses.VideoFiles
{
    public class HlsListFileClass : ResolutionDirectoryFileClass
    {
        public override string Name => "list";

        public HlsListFileClass(Guid key, Resolution resolution) : base(key, resolution)
        {
        }
    }
}
