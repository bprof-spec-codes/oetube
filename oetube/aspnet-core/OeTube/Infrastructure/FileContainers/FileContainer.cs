using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NUglify.JavaScript.Syntax;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Content;
using Volo.Abp.Http;
using Volo.Abp.IO;
using Volo.Abp.Uow;

namespace OeTube.Infrastructure.FileContainers
{
    public class FileContainer : IFileContainer
    {
        private readonly IBlobContainer _container;
        public string RootDirectory { get; }

        public string ContainerName { get; }

        public FileContainer(string containerName,
                             IBlobContainerFactory containerFactory,
                             IBlobFilePathCalculator calculator,
                             IBlobContainerConfigurationProvider provider)
        {
            ContainerName = containerName;
            _container = containerFactory.Create(ContainerName);
            RootDirectory = GetDirectory(calculator, provider);
        }
        private string GetDirectory(IBlobFilePathCalculator calculator, IBlobContainerConfigurationProvider provider)
        {
            string? absolutePath = Path.GetDirectoryName(calculator.Calculate(new BlobProviderGetArgs(ContainerName, provider.Get(ContainerName), "_")));
            if (absolutePath == null)
            {
                throw new NullReferenceException(nameof(GetDirectory));
            }
            return absolutePath;
        }
        public string GetAbsolutePath(string containerPath)
        {
            return Path.Combine(RootDirectory, containerPath);
        }
        public string GetContainerPath(string absolutePath)
        {
            return absolutePath.Replace(RootDirectory + Path.DirectorySeparatorChar, "");
        }
        public string? FindFirstOrNull(params string[] path)
        {
            return Find(path).FirstOrDefault();
        }
        public IEnumerable<string> Find(params string[] path)
        {
            if (path.Length == 0)
            {
                return Enumerable.Empty<string>();
            }
            string directory = path.Length == 1 ? path[0] : Path.Combine(path[..^1]);
            var file = path[^1];
            string absoluteDirectory = GetAbsolutePath(directory);
            string pattern = Path.HasExtension(file) ? file : file + "*";
            string[] files = Directory.GetFiles(absoluteDirectory, pattern, SearchOption.TopDirectoryOnly);
            return files.Select(GetContainerPath);
        }
        public async Task<bool> DeleteFirstAsync(params string[] path)
        {
            return await DeleteFirstAsync(path, default);
        }
        public async Task<bool> DeleteFirstAsync(string[] path, CancellationToken cancellationToken = default)
        {
            var result = FindFirstOrNull(path);
            if (result is null) return false;
            return await DeleteAsync(result, cancellationToken);
        }
        public async Task SaveAsync(ByteContent content, bool overrideExisting = false, CancellationToken cancellationToken = default)
        {
            await SaveAsync(content.Path, content.GetStream(), overrideExisting, cancellationToken);
        }
        public async Task SaveAsync(string name, Stream stream, bool overrideExisting = false, CancellationToken cancellationToken = default)
        {
            await _container.SaveAsync(name, stream, overrideExisting, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string name, CancellationToken cancellationToken = default)
        {
            var result = await _container.DeleteAsync(name, cancellationToken);
            if (!result)
            {
                return result;
            }
            var directory = Directory.GetParent(GetAbsolutePath(name));
            if (directory == null || directory.FullName == RootDirectory || directory.GetFiles().Length > 0)
            {
                return result;
            }
            Directory.Delete(directory.FullName, true);
            return result;
        }

        public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return _container.ExistsAsync(name, cancellationToken);
        }

        public Task<Stream> GetAsync(string name, CancellationToken cancellationToken = default)
        {
            return _container.GetAsync(name, cancellationToken);
        }
        public Task<Stream> GetOrNullAsync(string name, CancellationToken cancellationToken = default)
        {
            return _container.GetOrNullAsync(name, cancellationToken);
        }
        public async Task<ByteContent> GetContentAsync(string name, CancellationToken cancellationToken = default)
        {
            var stream = await GetAsync(name, cancellationToken);
            return await ByteContent.FromStreamAsync(name, stream, cancellationToken);
        }
        public async Task<ByteContent?> GetFirstOrNullAsync(params string[] path)
        {
            return await GetFirstOrNullAsync(path, default);
        }
        public async Task<ByteContent?> GetFirstOrNullAsync(string[] path, CancellationToken cancellationToken = default)
        {
            var result = FindFirstOrNull(path);
            if (result is null) return null;
            return await GetContentAsync(result, cancellationToken);
        }
    }
}
