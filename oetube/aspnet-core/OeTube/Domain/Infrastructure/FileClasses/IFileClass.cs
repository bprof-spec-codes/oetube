using OeTube.Domain.Infrastructure.Videos;

namespace OeTube.Domain.Infrastructure.FileClasses
{
   

    public interface IFileClass
    {
        IEnumerable<string> ExplicitFormats { get; }
        string Key { get; }
        long MaxFileSize { get; }
        string MimeTypeCategory { get; }
        string Name { get; }
        IEnumerable<string> SubPath { get; }

        void CheckContent(ByteContent content);
        string CombineWithKey(string localKeyPath);
        string GetAbsoluteKeyDirectory(string absoluteRootDirectory);
        string GetAbsolutePath(string absoluteRootDirectory, string? format = null);
        string GetAbsoluteSubPathDirectory(string absoluteRootDirectory);
        string GetKeyDirectory();
        string GetPath(string? format = null);
        string GetSubpathDirectory();
    }
    public interface IVideoFileClass : IFileClass
    { }
    public interface IGroupFileClass : IFileClass
    { }
    public interface IUserFileClass : IFileClass
    { }
    public interface IPlaylistFileClass : IFileClass
    { }

}
