using OeTube.Domain.Infrastructure.Videos;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.BlobStoring;
using OeTube.Domain.Infrastructure.FileContainers;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FileClassContainers
{
    public class FileContainer: ITransientDependency, IFileContainer
    { 
        public Type RelatedType { get; }
        private readonly IBlobContainer _container;
        public string RootDirectory { get; }

        public FileContainer(Type relatedType,
                             IBlobContainerFactory containerFactory,
                             IBlobFilePathCalculator calculator,
                             IBlobContainerConfigurationProvider provider)
        {
            RelatedType = relatedType;
            _container = containerFactory.Create(RelatedType.Name);
            RootDirectory = GetDirectory(calculator, provider);
        }

        public string? Find(FileClass fileClass)
        {
            var directory = fileClass.GetAbsoluteSubPathDirectory(RootDirectory);
            var pattern = fileClass.Name + ".*";
            if (!Directory.Exists(directory))
            {
                return null;
            }
            return Directory.GetFiles(directory, pattern, SearchOption.TopDirectoryOnly)
                           .Select(f => ToLocalKeyPath(fileClass.Key, f))
                           .FirstOrDefault();
        }

        public async Task<ByteContent?> GetOrNullAsync(FileClass file, CancellationToken cancellationToken = default)
        {
            var result = Find(file);
            if (result is null) return null;
            var stream = await _container.GetAsync(file.CombineWithKey(result), cancellationToken);

            return await ByteContent.FromStreamAsync(Path.GetExtension(result), stream, cancellationToken);
        }
        public async Task<ByteContent> GetAsync(FileClass file, CancellationToken cancellationToken = default)
        {
            var result = await GetOrNullAsync(file, cancellationToken);
            if (result is null) throw new ArgumentException(file.GetPath(), nameof(file));
            return result;
        }
        public IEnumerable<string> GetFiles(object key)
        {
            string keyStr = KeyToString(key);
            return Directory.EnumerateFiles(Path.Combine(RootDirectory, keyStr), "*.*", SearchOption.AllDirectories)
                             .Select(f => ToLocalKeyPath(keyStr, f));
        }

        public async Task<bool> DeleteAsync(FileClass file, CancellationToken cancellationToken = default)
        {
            var result = Find(file);
            if (result is null) return false;

            await _container.DeleteAsync(file.CombineWithKey(result), cancellationToken);
            await Try(() =>
            {
                var directory = file.GetAbsoluteKeyDirectory(RootDirectory);
                if (Directory.Exists(directory) && !Directory.EnumerateFileSystemEntries(directory).Any())
                {
                    Directory.Delete(directory, true);
                }
            }, 100, 5, cancellationToken);
            return true;
        }

        public async Task DeleteKeyAsync(object key, CancellationToken cancellationToken = default)
        {
            var keyStr = KeyToString(key);

            await Try(() =>
            {
                var path = Path.Combine(RootDirectory, keyStr);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }, 100, 5, cancellationToken);
        }

        public async Task SaveAsync(FileClass fileClass, ByteContent content, CancellationToken cancellationToken = default)
        {
            fileClass.CheckContent(content);
            await DeleteAsync(fileClass, cancellationToken);
            await _container.SaveAsync(fileClass.GetPath(content.Format), content.Bytes, false, cancellationToken);
        }


        protected virtual string GetDirectory(IBlobFilePathCalculator calculator, IBlobContainerConfigurationProvider provider)
        {
            string? absolutePath = Path.GetDirectoryName(calculator.Calculate(new BlobProviderGetArgs(RelatedType.Name, provider.Get(RelatedType.Name), "_")));
            return absolutePath ?? throw new NullReferenceException(nameof(GetDirectory));
        }
        private string ToLocalKeyPath(string keyStr, string absolutePath)
        {
            return absolutePath.Replace(Path.Combine(RootDirectory, keyStr) + Path.DirectorySeparatorChar, "");
        }


        private static string KeyToString(object key)
        {
            var keyStr = key?.ToString();
            if (string.IsNullOrWhiteSpace(keyStr))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return keyStr;

        }
        private static async Task Try(Action action, int delayMs, int maxTry, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return;
            int count = 0;
            do
            {
                try
                {
                    action();
                    count = maxTry;
                }
                catch
                {
                    maxTry++;
                    await Task.Delay(delayMs, cancellationToken);
                }
            } while (count < maxTry && !cancellationToken.IsCancellationRequested);
        }
    }

}
