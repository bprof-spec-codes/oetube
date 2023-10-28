using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.VideoStorage;
using OeTube.Infrastructure.FileContainers;
using System.Diagnostics;

namespace OeTube.Infrastructure.Videos.VideoStorages
{
    public class VideoStorageDelete : IVideoStorageDelete
    {
        private readonly IFileContainer _container;
        private readonly IVideoStoragePath _path;

        public VideoStorageDelete(IFileContainer container, IVideoStoragePath path)
        {
            _container = container;
            _path = path;
        }

        public async Task<bool> DeleteSourceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var path = _path.GetSourcePath(id);
            return await _container.DeleteFirstAsync(path, cancellationToken);
        }

        public async Task<bool> DeleteResizedAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default)
        {
            var path = _path.GetResizedPath(id, resolution);
            return await _container.DeleteFirstAsync(path, cancellationToken);
        }

        private async Task Try(Action action, int delayMs, int maxTry, bool exceptionToDebug = false, CancellationToken cancellationToken = default, string? name = null)
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
                catch (Exception ex)
                {
                    Debug.WriteLineIf(exceptionToDebug, $"try [{name} - {count}]: " + ex.Message);
                    maxTry++;
                    await Task.Delay(delayMs, cancellationToken);
                }
            } while (count < maxTry && !cancellationToken.IsCancellationRequested);
        }

        public async Task DeleteAllAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await Try(() =>
            {
                var path = _container.GetAbsolutePath(id.ToString());
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }, 100, 5, true, cancellationToken, nameof(DeleteAllAsync));
        }
    }
}