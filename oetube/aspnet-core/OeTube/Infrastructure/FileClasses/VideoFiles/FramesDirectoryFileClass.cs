namespace OeTube.Infrastructure.FileClasses.VideoFiles
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
