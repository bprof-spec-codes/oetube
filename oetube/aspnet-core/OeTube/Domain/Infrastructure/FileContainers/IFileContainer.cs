using OeTube.Domain.FilePaths;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Infrastructure.Videos;

namespace OeTube.Domain.Infrastructure.FileContainers
{
   
    public interface IReadOnlyFileContainer
    {
        Type RelatedType { get; }
        string RootDirectory { get; }
        Task<ByteContent> GetFileAsync(IFilePath file, CancellationToken cancellationToken = default);
        Task<ByteContent?> GetFileOrNullAsync(IFilePath file, CancellationToken cancellationToken = default);
        string? FindDefaultFile<TDefaultFilePath>() where TDefaultFilePath : IDefaultFilePath;
        string? FindFile(IFilePath path);
        Task<ByteContent?> GetDefaultFileOrNullAsync<TDefaultFilePath>(CancellationToken cancellationToken = default) where TDefaultFilePath : IDefaultFilePath;
        IEnumerable<string> GetFiles(object key);
        Task<ByteContent> GetFileOrDefault<TDefaultFilePath>(TDefaultFilePath path, CancellationToken cancellationToken = default) where TDefaultFilePath : IDefaultFilePath;
    }

    public interface IFileContainer :IReadOnlyFileContainer
    {
        Task<bool> DeleteDefaultFileAsync<TDefaultFilePath>(CancellationToken cancellationToken = default) where TDefaultFilePath : IDefaultFilePath;
        Task<bool> DeleteFileAsync(IFilePath path, CancellationToken cancellationToken = default);
        Task DeleteKeyFilesAsync(object key, CancellationToken cancellationToken = default);
        Task SaveDefaultFileAsync<TDefaultFilePath>(ByteContent content, CancellationToken cancellationToken = default) where TDefaultFilePath : IDefaultFilePath;
        Task SaveFileAsync(IFilePath path, ByteContent content, CancellationToken cancellationToken = default);
    }
   

}