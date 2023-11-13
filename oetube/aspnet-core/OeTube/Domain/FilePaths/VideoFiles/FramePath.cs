namespace OeTube.Domain.FilePaths.VideoFiles
{
    public class FramePath : FramesDirectoryPath
    {
        public override string Name { get; }

        public FramePath(Guid key, int index) : base(key)
        {
            Name = index.ToString();
        }
    }
}
