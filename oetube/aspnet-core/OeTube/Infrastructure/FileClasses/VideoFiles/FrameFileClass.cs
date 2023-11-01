namespace OeTube.Infrastructure.FileClasses.VideoFiles
{
    public class FrameFileClass : FramesDirectoryFileClass
    {
        public override string Name { get; }

        public FrameFileClass(Guid key, int index) : base(key)
        {
            Name = index.ToString();
        }
    }
}
