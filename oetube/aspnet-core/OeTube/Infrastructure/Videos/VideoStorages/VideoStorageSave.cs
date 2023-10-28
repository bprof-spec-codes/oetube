using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Infrastructure.VideoStorage;
using OeTube.Infrastructure.FileContainers;

namespace OeTube.Infrastructure.Videos.VideoStorages
{
    public class VideoStorageSave : IVideoStorageSave
    {
        private readonly IFileContainer _container;
        private readonly IVideoStoragePath _path;

        public VideoStorageSave(IFileContainer container, IVideoStoragePath path)
        {
            _container = container;
            _path = path;
        }

        public async Task SaveSourceAsync(Guid id, ByteContent content, CancellationToken cancellationToken = default)
        {
            string path = _path.GetSourcePath(id, content.Format);
            await _container.SaveAsync(content.WithNewPath(path), true, cancellationToken);
        }

        public async Task SaveResizedAsync(Guid id, Resolution resolution, ByteContent content, CancellationToken cancellationToken = default)
        {
            string path = _path.GetResizedPath(id, resolution, content.Format);
            await _container.SaveAsync(content.WithNewPath(path), true, cancellationToken);
        }

        public async Task SaveFrameAsync(Guid id, int index, ByteContent content, CancellationToken cancellationToken = default)
        {
            var path = _path.GetFramePath(id, index, content.Format);
            await _container.SaveAsync(content.WithNewPath(path), true, cancellationToken);
        }

        public async Task SaveSelectedFrameAsync(Guid id, int index, CancellationToken cancellationToken = default)
        {
            var path = _path.GetFramePath(id, index);
            var content = await _container.GetFirstOrNullAsync(path, cancellationToken) ?? throw new ArgumentException(null, nameof(index));

            var selectedPath = _path.GetSelectedFramePath(id, content.Format);
            await _container.SaveAsync(content.WithNewPath(selectedPath), true, cancellationToken);
        }

        public async Task SaveHlsListAsync(Guid id, Resolution resolution, ByteContent content, CancellationToken cancellationToken = default)
        {
            var path = _path.GetHlsListPath(id, resolution);
            await _container.SaveAsync(content.WithNewPath(path), true, cancellationToken);
        }

        public async Task SaveHlsSegmentAsync(Guid id, Resolution resolution, int segment, ByteContent content, CancellationToken cancellationToken = default)
        {
            var path = _path.GetHlsSegmentPath(id, resolution, segment);
            await _container.SaveAsync(content.WithNewPath(path), true, cancellationToken);
        }
    }
}