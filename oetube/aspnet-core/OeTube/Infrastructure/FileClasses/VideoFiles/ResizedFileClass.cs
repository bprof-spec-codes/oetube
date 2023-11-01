using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.FileClasses.VideoFiles
{
    public class ResizedFileClass : ResolutionDirectoryFileClass
    {
        public override string Name => "resized";
        public ResizedFileClass(Guid key, Resolution resolution) : base(key, resolution)
        {
        }
    }
}
