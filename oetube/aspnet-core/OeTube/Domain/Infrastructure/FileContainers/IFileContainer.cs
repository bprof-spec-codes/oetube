using OeTube.Domain.Infrastructure.Videos;

namespace OeTube.Domain.Infrastructure.FileContainers
{
    public interface IReadOnlyFileContainer
    {
        Type RelatedType { get; }
        string RootDirectory { get; }

        string? Find(FileClass fileClass);
        Task<ByteContent> GetAsync(FileClass file, CancellationToken cancellationToken = default);
        IEnumerable<string> GetFiles(object key);
        Task<ByteContent?> GetOrNullAsync(FileClass file, CancellationToken cancellationToken = default);
    }

    public interface IFileContainer :IReadOnlyFileContainer
    {
      
        Task<bool> DeleteAsync(FileClass file, CancellationToken cancellationToken = default);
        Task DeleteKeyAsync(object key, CancellationToken cancellationToken = default);
        Task SaveAsync(FileClass fileClass, ByteContent content, CancellationToken cancellationToken = default);
    }
}