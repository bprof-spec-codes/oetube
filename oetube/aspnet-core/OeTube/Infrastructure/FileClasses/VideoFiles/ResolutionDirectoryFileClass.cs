using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.FileClasses.VideoFiles
{
    public abstract class ResolutionDirectoryFileClass : VideoFileClass
    {
        public override IEnumerable<string> SubPath
        {
            get
            {
                yield return Resolution.ToString();
            }
        }

        public Resolution Resolution { get; }
        protected ResolutionDirectoryFileClass(Guid key, Resolution resolution) : base(key)
        {
            Resolution = resolution;
        }
    }
}
