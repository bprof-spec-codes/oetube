using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Infrastructure.VideoStorage;
using OeTube.Infrastructure.FileContainers;

namespace OeTube.Infrastructure.Videos.VideoStorages
{
    public class VideoStorageRead : IVideoStorageRead
    {
        private readonly IFileContainer _container;
        private readonly IVideoStoragePath _path;

        public VideoStorageRead(IFileContainer container, IVideoStoragePath path)
        {
            _container = container;
            _path = path;
        }

        public async Task<ByteContent> ReadSourceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            string path = _path.GetSourcePath(id);
            return await _container.GetFirstOrNullAsync(path, cancellationToken) ?? throw new NullReferenceException();
        }

        public IEnumerable<Resolution> GetResolutions(Guid id)
        {
            var path = _container.GetAbsolutePath(id.ToString());
            foreach (var item in Directory.EnumerateDirectories(path))
            {
                if (Resolution.TryParse(item, out Resolution resolution))
                {
                    yield return resolution;
                }
            }
        }

        public IEnumerable<int> GetFrames(Guid id)
        {
            var path = _container.GetAbsolutePath(_path.GetFramesDirectoryPath(id));
            return Directory.EnumerateFiles(path).Select(f => Path.GetFileNameWithoutExtension(f))
                            .Where(f => f != _path.SelectedFrameName)
                            .Select(int.Parse);
        }

        public async Task<ByteContent> ReadResizedAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default)
        {
            string path = _path.GetResizedPath(id, resolution);
            return await _container.GetFirstOrNullAsync(path, cancellationToken) ?? throw new NullReferenceException();
        }

        public async Task<ByteContent> ReadFrameAsync(Guid id, int index, CancellationToken cancellationToken = default)
        {
            var path = _path.GetFramePath(id, index);
            return await _container.GetFirstOrNullAsync(path, cancellationToken) ?? throw new NullReferenceException();
        }

        public async Task<ByteContent> ReadSelectedFrameAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var path = _path.GetSelectedFramePath(id);
            return await _container.GetFirstOrNullAsync(path, cancellationToken) ?? throw new NullReferenceException();
        }

        public async Task<ByteContent> ReadHlsListAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default)
        {
            var path = _path.GetHlsListPath(id, resolution);
            var content = await _container.GetContentAsync(path, cancellationToken);
            return content.WithNewContentType("application/x-mpegURL");
        }

        public IEnumerable<int> GetHlsSegments(Guid id, Resolution resolution)
        {
            var path = _container.GetAbsolutePath(_path.GetResolutionDirectoryPath(id, resolution));
            return Directory.EnumerateFiles(path).Select(f => Path.GetFileNameWithoutExtension(f))
                            .Where(f => f != _path.HlsListName)
                            .Select(int.Parse);
        }

        public async Task<ByteContent> ReadHlsSegmentAsync(Guid id, Resolution resolution, int segment, CancellationToken cancellationToken = default)
        {
            var path = _path.GetHlsSegmentPath(id, resolution, segment);
            return await _container.GetContentAsync(path, cancellationToken);
        }
    }
}