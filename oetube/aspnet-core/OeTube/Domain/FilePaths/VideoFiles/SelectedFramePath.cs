namespace OeTube.Domain.FilePaths.VideoFiles
{
    public class SelectedFramePath : FramesDirectoryPath
    {
        public SelectedFramePath(Guid key) : base(key)
        {
        }

        public override string Name => "selected";
    }
}