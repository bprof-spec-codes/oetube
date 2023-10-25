using OeTube.Domain.Entities.Videos;
using OeTube.Infrastructure.FileContainers;

namespace OeTube.Infrastructure.VideoStorages
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
        public async Task<bool> SourceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var path = _path.Source(id);
            return await _container.DeleteFirstAsync(path, cancellationToken);
        }
        public async Task<bool> ResizedAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default)
        {
            var path = _path.Resized(id, resolution);
            return await _container.DeleteFirstAsync(path, cancellationToken);
        }
        public async Task<bool> FrameAsync(Guid id, int index, CancellationToken cancellationToken = default)
        {
            var path = _path.Frame(id, index);
            return await _container.DeleteFirstAsync(path, cancellationToken);
        }
        public async Task<bool> SelectedFrameAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var path = _path.SelectedFrame(id);
            return await _container.DeleteFirstAsync(path,cancellationToken);
        }
        public async Task<bool> HlsListAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default)
        {
            var path = _path.HlsList(id, resolution);
            return await _container.DeleteAsync(path, cancellationToken);
        }
        public async Task<bool> HlsSegmentAsync(Guid id, Resolution resolution, int segment, CancellationToken cancellationToken = default)
        {
            var path = _path.HlsSegment(id, resolution, segment);
            return await _container.DeleteAsync(path, cancellationToken);
        }
        public void DeleteAll(Guid id)
        {
            try
            {
                Directory.Delete(_container.GetAbsolutePath(id.ToString()), true);
            }
            finally
            {
                Directory.Delete(_container.GetAbsolutePath(id.ToString()), true);
            }
        }

    }

}
