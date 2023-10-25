using Volo.Abp.BlobStoring;

namespace OeTube.Infrastructure.FileContainers
{
    public interface IFileContainer:IBlobContainer
    {
        string ContainerName { get; }
        string RootDirectory { get; }

        Task<bool> DeleteFirstAsync(string path);
        Task<bool> DeleteFirstAsync(string path, CancellationToken cancellationToken = default);
        IEnumerable<string> Find(string path);
        string? FindFirstOrNull(string path);
        string GetAbsolutePath(string containerPath);
        string GetContainerPath(string absolutePath);
        Task<ByteContent> GetContentAsync(string name, CancellationToken cancellationToken = default);
        Task<ByteContent?> GetFirstOrNullAsync(string path);
        Task<ByteContent?> GetFirstOrNullAsync(string path, CancellationToken cancellationToken = default);
        Task SaveAsync(ByteContent content, bool overrideExisting = false, CancellationToken cancellationToken = default);
    }
}