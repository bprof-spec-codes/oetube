using OeTube.Domain.Infrastructure.Videos;

namespace OeTube.Domain.FilePaths
{
    public interface IDefaultFilePath:IFilePath
    {
        public static abstract string GetDefaultPath(string? format = null); 
    }

    public interface IFilePath
    {
        string Key { get; }
        string Name { get; }
        IEnumerable<string> SubPath { get; }
        string CombineWithKey(string localKeyPath);
        string GetAbsoluteKeyDirectory(string absoluteRootDirectory);
        string GetAbsolutePath(string absoluteRootDirectory, string? format = null);
        string GetAbsoluteSubPathDirectory(string absoluteRootDirectory);
        string GetKeyDirectory();
        string GetPath(string? format = null);
        string GetSubpathDirectory();
    }
   
}
