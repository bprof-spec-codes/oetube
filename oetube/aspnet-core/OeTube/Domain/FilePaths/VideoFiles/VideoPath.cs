namespace OeTube.Domain.FilePaths.VideoFiles
{
    public abstract class VideoPath : FilePath
    {
        public override string Key { get; }

        public VideoPath(Guid key)
        {
            Key = key.ToString() ?? string.Empty;
        }
    }
}