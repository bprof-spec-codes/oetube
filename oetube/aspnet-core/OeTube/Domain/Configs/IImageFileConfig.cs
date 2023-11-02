namespace OeTube.Domain.Configs
{
    public interface IImageFileConfig
    {
        public int MaxWidth { get; }
        public int MaxHeight { get; }
        public string OutputFormat { get; }
    }
    public interface ISourceImageFileConfig:IImageFileConfig
    {
     
    }
    public interface IThumbnailImageFileConfig:IImageFileConfig
    {

    }
}