using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.FilePaths.VideoFiles
{
    public abstract class ResolutionDirectoryPath : VideoPath
    {
        public override IEnumerable<string> SubPath
        {
            get
            {
                yield return Resolution.ToString();
            }
        }

        public Resolution Resolution { get; }
        protected ResolutionDirectoryPath(Guid key, Resolution resolution) : base(key)
        {
            Resolution = resolution;
        }
    }
}
