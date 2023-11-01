namespace OeTube.Infrastructure.FileClasses.VideoFiles
{
    public class SourceFileClass : VideoFileClass
    {
        public SourceFileClass(Guid key) : base(key)
        {
        }

        public override long MaxFileSize => 1024L * 1024 * 1024;
        public override string Name => "source";
    }
}
