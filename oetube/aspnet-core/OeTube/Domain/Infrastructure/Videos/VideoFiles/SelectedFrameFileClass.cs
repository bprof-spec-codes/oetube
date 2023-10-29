namespace OeTube.Domain.Infrastructure.Videos.VideoFiles
{
    public class SelectedFrameFileClass : FramesDirectoryFileClass
    {
        public SelectedFrameFileClass(Guid key) : base(key)
        {
        }

        public override string Name => "selected";

    }
}
