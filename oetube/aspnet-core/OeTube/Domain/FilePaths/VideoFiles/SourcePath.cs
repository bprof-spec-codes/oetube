namespace OeTube.Domain.FilePaths.VideoFiles
{
    public class SourcePath : VideoPath
    {
        public SourcePath(Guid key) : base(key)
        {
        }

        public override string Name => "source";
    }
}