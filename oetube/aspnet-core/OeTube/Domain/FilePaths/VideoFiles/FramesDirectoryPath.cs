namespace OeTube.Domain.FilePaths.VideoFiles
{
    public abstract class FramesDirectoryPath : VideoPath
    {
        public override IEnumerable<string> SubPath
        {
            get
            {
                yield return "frames";
            }
        }

        protected FramesDirectoryPath(Guid key) : base(key)
        {
        }
    }
}