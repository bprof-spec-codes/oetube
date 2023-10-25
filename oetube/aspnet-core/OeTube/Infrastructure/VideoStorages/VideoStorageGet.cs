using OeTube.Domain.Entities.Videos;
using OeTube.Infrastructure.FileContainers;

namespace OeTube.Infrastructure.VideoStorages
{
    public class VideoStorageGet : IVideoStorageGet
    {
        private readonly IFileContainer _container;
        private readonly IVideoStoragePath _path;

        public VideoStorageGet(IFileContainer container, IVideoStoragePath path)
        {
            _container = container;
            _path = path;
        }

        public async Task<ByteContent> SourceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            string path = _path.Source(id);
            return await _container.GetFirstOrNullAsync(path, cancellationToken) ?? throw new NullReferenceException();
        }
        public IEnumerable<int> Frames(Guid id)
        {
            var path = _container.GetAbsolutePath(_path.FramesDirectory(id));
            return Directory.EnumerateFiles(path).Select(f =>Path.GetFileNameWithoutExtension(f))
                            .Where(f => f != _path.SelectedFrameName)
                            .Select(int.Parse);
        }
        public async Task<ByteContent> ResizedAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default)
        {
            string path = _path.Resized(id, resolution);
            return await _container.GetFirstOrNullAsync(path, cancellationToken) ?? throw new NullReferenceException();
        }
        public async Task<ByteContent> FrameAsync(Guid id, int index, CancellationToken cancellationToken = default)
        {
            var path = _path.Frame(id, index);
            return await _container.GetFirstOrNullAsync(path, cancellationToken) ?? throw new NullReferenceException();
        }
        public async Task<ByteContent> SelectedFrameAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var path = _path.SelectedFrame(id);
            return await _container.GetFirstOrNullAsync(path, cancellationToken) ?? throw new NullReferenceException();
        }
        public async Task<ByteContent> HlsListAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default)
        {
            var path = _path.HlsList(id, resolution);
            var content= await _container.GetContentAsync(path, cancellationToken);
            return content.WithNewContentType("application/x-mpegURL");
        }

        public IEnumerable<int> HlsSegments(Guid id,Resolution resolution)
        {
            var path = _container.GetAbsolutePath(_path.ResolutionDirectory(id,resolution));
            return Directory.EnumerateFiles(path).Select(f => Path.GetFileNameWithoutExtension(f))
                            .Where(f => f != _path.HlsListName)
                            .Select(int.Parse);
        }

        public async Task<ByteContent> HlsSegmentAsync(Guid id, Resolution resolution, int segment, CancellationToken cancellationToken = default)
        {
            var path = _path.HlsSegment(id, resolution, segment);
            return await _container.GetContentAsync(path, cancellationToken);
        }

    }

}
