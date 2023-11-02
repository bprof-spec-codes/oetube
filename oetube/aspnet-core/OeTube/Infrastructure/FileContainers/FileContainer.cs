using OeTube.Domain.Infrastructure.Videos;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.BlobStoring;
using OeTube.Domain.Infrastructure.FileContainers;
using Volo.Abp.DependencyInjection;
using OeTube.Domain.FilePaths;
using OeTube.Domain.Infrastructure.FileHandlers;
using System.Runtime.CompilerServices;
using System.IO;
using OeTube.Domain.Infrastructure;

namespace OeTube.Infrastructure.FileContainers
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

        public string? FindFile(IFilePath path)
        {
            var directory = path.GetAbsoluteSubPathDirectory(RootDirectory);
            var pattern = path.Name + ".*";
            if (!Directory.Exists(directory))
            {
                return null;
            }
            return Directory.EnumerateFiles(directory, pattern, SearchOption.TopDirectoryOnly)
                           .Select(f => ToLocalKeyPath(path.Key, f))
                           .FirstOrDefault();
        }

        public async Task<ByteContent?> GetFileOrNullAsync(IFilePath path, CancellationToken cancellationToken = default)
        {
            var result = FindFile(path);
            if (result is null) return null;
            var stream = await _container.GetAsync(path.CombineWithKey(result), cancellationToken);

            return await ByteContent.FromStreamAsync(Path.GetExtension(result), stream, cancellationToken);
        }
        public async Task<ByteContent> GetFileAsync(IFilePath path, CancellationToken cancellationToken = default)
        {
            var result = await GetFileOrNullAsync(path, cancellationToken);
            if (result is null) throw new ArgumentException(path.GetPath(), nameof(path));
            return result;
        }
        public IEnumerable<string> GetFiles(object key)
        {
            string keyStr = KeyToString(key);
            return Directory.EnumerateFiles(Path.Combine(RootDirectory, keyStr), "*.*", SearchOption.AllDirectories)
                             .Select(f => ToLocalKeyPath(keyStr, f));
        }

        public async Task<bool> DeleteFileAsync(IFilePath path, CancellationToken cancellationToken = default)
        {
            var result = FindFile(path);
            if (result is null) return false;

            await _container.DeleteAsync(path.CombineWithKey(result), cancellationToken);
            await Try(() =>
            {
                var directory = path.GetAbsoluteKeyDirectory(RootDirectory);
                if (Directory.Exists(directory) && !Directory.EnumerateFileSystemEntries(directory).Any())
                {
                    Directory.Delete(directory, true);
                }
            }, 100, 5, cancellationToken);
            return true;
        }

        public async Task DeleteKeyFilesAsync(object key, CancellationToken cancellationToken = default)
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
     
        public async Task SaveFileAsync(IFilePath path, ByteContent content, CancellationToken cancellationToken = default)
        {
            await DeleteFileAsync(path, cancellationToken);
            await _container.SaveAsync(path.GetPath(content.Format), content.Bytes, false, cancellationToken);
        }

        public async Task<ByteContent> GetFileOrDefault<TDefaultFilePath>(TDefaultFilePath path,CancellationToken cancellationToken=default)
            where TDefaultFilePath:IDefaultFilePath
        {
            var result=await GetFileOrNullAsync(path,cancellationToken);
            if(result is null)
            {
               var defaultFile=await GetDefaultFileOrNullAsync<TDefaultFilePath>(cancellationToken) ?? throw new ArgumentException(TDefaultFilePath.GetDefaultPath(), nameof(path));
                return defaultFile;
            }
            return result;
        }
        public async Task<ByteContent?> GetDefaultFileOrNullAsync<TDefaultFilePath>(CancellationToken cancellationToken=default)
        where TDefaultFilePath:IDefaultFilePath
        {
            var result = FindDefaultFile<TDefaultFilePath>();
            if (result is null) return null;
            var stream = await _container.GetAsync(result, cancellationToken);
            return await ByteContent.FromStreamAsync(Path.GetExtension(result), stream, cancellationToken);
        }
        public async Task SaveDefaultFileAsync<TDefaultFilePath>(ByteContent content,CancellationToken cancellationToken = default)
        where TDefaultFilePath:IDefaultFilePath
        {
            await DeleteDefaultFileAsync<TDefaultFilePath>(cancellationToken);
            await _container.SaveAsync(TDefaultFilePath.GetDefaultPath(content.Format), content.Bytes, false, cancellationToken);
        }
        public async Task<bool> DeleteDefaultFileAsync<TDefaultFilePath>(CancellationToken cancellationToken=default)
        where TDefaultFilePath:IDefaultFilePath
        {
            var result = FindDefaultFile<TDefaultFilePath>();
            if (result is null) return false;
            return await _container.DeleteAsync(result, cancellationToken);
        }
        public string? FindDefaultFile<TDefaultFilePath>()
        where TDefaultFilePath:IDefaultFilePath
        {
            var directory = RootDirectory;
            var pattern =TDefaultFilePath.GetDefaultPath() + "*";
            if (!Directory.Exists(directory))
            {
                return null;
            }
            return Directory.EnumerateFiles(directory, pattern, SearchOption.TopDirectoryOnly)
                           .Select(f=>f.Replace(RootDirectory+Path.DirectorySeparatorChar,""))
                           .FirstOrDefault();
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
