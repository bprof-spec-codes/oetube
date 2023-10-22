using OeTube.Domain.Entities.Videos;
using OeTube.Infrastructure.FileContainers;

namespace OeTube.Infrastructure.VideoStorages
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

        public async Task SourceAsync(Guid id, ByteContent content, CancellationToken cancellationToken = default)
        {
            string path = _path.Source(id, content.Format);
            await _container.SaveAsync(content.WithNewPath(path), true, cancellationToken);
        }
        public async Task ResizedAsync(Guid id, Resolution resolution, ByteContent content, CancellationToken cancellationToken = default)
        {
            string path = _path.Resized(id, resolution, content.Format);
            await _container.SaveAsync(content.WithNewPath(path), true, cancellationToken);
        }
        public async Task FrameAsync(Guid id, int index, ByteContent content, CancellationToken cancellationToken = default)
        {
            var path = _path.Frame(id, index, content.Format);
            await _container.SaveAsync(content.WithNewPath(path), true, cancellationToken);
        }
        public async Task SelectedFrameAsync(Guid id, int index,CancellationToken cancellationToken=default)
        {
            var path = _path.Frame(id, index);
            var content = await _container.GetFirstOrNullAsync(path,cancellationToken)??throw new ArgumentException(null,nameof(index));

            var selectedPath = _path.SelectedFrame(id,content.Format);
            await _container.SaveAsync(content.WithNewPath(selectedPath), true, cancellationToken);
        }
        public async Task HlsListAsync(Guid id, Resolution resolution, ByteContent content, CancellationToken cancellationToken = default)
        {
            var path = _path.HlsList(id, resolution);
            await _container.SaveAsync(content.WithNewPath(path), true, cancellationToken);
        }
        public async Task HlsSegmentAsync(Guid id, Resolution resolution, int segment, ByteContent content, CancellationToken cancellationToken = default)
        {
            var path = _path.HlsSegment(id, resolution, segment);
            await _container.SaveAsync(content.WithNewPath(path), true,cancellationToken);
        }

    }

}
