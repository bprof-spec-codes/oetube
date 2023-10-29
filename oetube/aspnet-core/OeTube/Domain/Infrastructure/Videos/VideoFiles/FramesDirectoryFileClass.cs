namespace OeTube.Domain.Infrastructure.Videos.VideoFiles
{

    public abstract class FramesDirectoryFileClass : VideoFileClass
    {
        public override IEnumerable<string> SubPath
        {
            get
            {
                yield return "frames";
            }
        }
        protected FramesDirectoryFileClass(Guid key) : base(key)
        {
        }
    }
}
