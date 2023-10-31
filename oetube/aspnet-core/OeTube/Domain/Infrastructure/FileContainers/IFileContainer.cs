using OeTube.Domain.Infrastructure.FileClasses;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Infrastructure.FileClasses;

namespace OeTube.Domain.Infrastructure.FileContainers
{
    public interface IFileProvider
    {
        Task<ByteContent> GetFileAsync(IFileClass file, CancellationToken cancellationToken = default);
        Task<ByteContent?> GetFileOrNullAsync(IFileClass file, CancellationToken cancellationToken = default);
    }
 
    public interface IReadOnlyFileContainer:IFileProvider
    {
        Type RelatedType { get; }
        string RootDirectory { get; }

        string? Find(IFileClass fileClass);
        IEnumerable<string> GetFiles(object key);
    }

    public interface IFileContainer :IReadOnlyFileContainer
    {
      
        Task<bool> DeleteFileAsync(IFileClass file, CancellationToken cancellationToken = default);
        Task DeleteKeyFilesAsync(object key, CancellationToken cancellationToken = default);
        Task SaveFileAsync(IFileClass fileClass, ByteContent content, CancellationToken cancellationToken = default);
    }
    public interface IFileProvider<TFileClass>
     where TFileClass : IFileClass
    {
        Task<ByteContent> GetFileAsync(TFileClass file, CancellationToken cancellationToken = default);
        Task<ByteContent?> GetFileOrNullAsync(TFileClass file, CancellationToken cancellationToken = default);
    }
}