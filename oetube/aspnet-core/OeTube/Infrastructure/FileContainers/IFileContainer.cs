using Volo.Abp.BlobStoring;

namespace OeTube.Infrastructure.FileContainers
{
    public interface IFileContainer:IBlobContainer
    {
        string ContainerName { get; }
        string RootDirectory { get; }

        Task<bool> DeleteFirstAsync(params string[] path);
        Task<bool> DeleteFirstAsync(string[] path, CancellationToken cancellationToken = default);
        IEnumerable<string> Find(params string[] path);
        string? FindFirstOrNull(params string[] path);
        string GetAbsolutePath(string containerPath);
        string GetContainerPath(string absolutePath);
        Task<ByteContent> GetContentAsync(string name, CancellationToken cancellationToken = default);
        Task<ByteContent?> GetFirstOrNullAsync(params string[] path);
        Task<ByteContent?> GetFirstOrNullAsync(string[] path, CancellationToken cancellationToken = default);
        Task SaveAsync(ByteContent content, bool overrideExisting = false, CancellationToken cancellationToken = default);
    }
}