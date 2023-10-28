using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Infrastructure.VideoStorage;
using OeTube.Infrastructure.FileContainers;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.VideoStorages
{
    public class VideoStorage : IVideoStoragePath, IVideoStorageRead, IVideoStorageSave, IVideoStorageDelete, IVideoStorage, ITransientDependency
    {
        private readonly IFileContainer _container;
        private readonly IVideoStoragePath _pathMethods;
        private readonly IVideoStorageRead _readMethods;
        private readonly IVideoStorageSave _saveMethods;
        private readonly IVideoStorageDelete _deleteMethods;

        public VideoStorage(IFileContainerFactory containerFactory, IVideoStorageMethodFactory methodFactory)
        {
            _container = containerFactory.Create("videos");
            _pathMethods = methodFactory.CreatePath();
            _saveMethods = methodFactory.CreateSave(_container, _pathMethods);
            _readMethods = methodFactory.CreateRead(_container, _pathMethods);
            _deleteMethods = methodFactory.CreateDelete(_container, _pathMethods);
        }

        public string SelectedFrameName => _pathMethods.SelectedFrameName;

        public string HlsListName => _pathMethods.HlsListName;

        public string HlsListFormat => _pathMethods.HlsListFormat;

        public string HlsSegmentFormat => _pathMethods.HlsSegmentFormat;

        public string SourceName => _pathMethods.SourceName;

        public string ResizedName => _pathMethods.ResizedName;

        public void DeleteAllAsync(Guid id)
        {
            _deleteMethods.DeleteAllAsync(id);
        }

        public Task<bool> DeleteResizedAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default)
        {
            return _deleteMethods.DeleteResizedAsync(id, resolution, cancellationToken);
        }

        public Task<bool> DeleteSourceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _deleteMethods.DeleteSourceAsync(id, cancellationToken);
        }

        public string GetFramePath(Guid id, int index, string? format = null)
        {
            return _pathMethods.GetFramePath(id, index, format);
        }

        public IEnumerable<int> GetFrames(Guid id)
        {
            return _readMethods.GetFrames(id);
        }

        public string GetFramesDirectoryPath(Guid id)
        {
            return _pathMethods.GetFramesDirectoryPath(id);
        }

        public string GetHlsListPath(Guid id, Resolution resolution)
        {
            return _pathMethods.GetHlsListPath(id, resolution);
        }

        public string GetHlsSegmentPath(Guid id, Resolution resolution, int segment)
        {
            return _pathMethods.GetHlsSegmentPath(id, resolution, segment);
        }

        public string GetResizedPath(Guid id, Resolution resolution, string? format = null)
        {
            return _pathMethods.GetResizedPath(id, resolution, format);
        }

        public string GetResolutionDirectoryPath(Guid id, Resolution resolution)
        {
            return _pathMethods.GetResolutionDirectoryPath(id, resolution);
        }

        public string GetSelectedFramePath(Guid id, string? format = null)
        {
            return _pathMethods.GetSelectedFramePath(id, format);
        }

        public string GetSourcePath(Guid id, string? format = null)
        {
            return _pathMethods.GetSourcePath(id, format);
        }

        public Task<ByteContent> ReadFrameAsync(Guid id, int index, CancellationToken cancellationToken = default)
        {
            return _readMethods.ReadFrameAsync(id, index, cancellationToken);
        }

        public Task<ByteContent> ReadHlsListAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default)
        {
            return _readMethods.ReadHlsListAsync(id, resolution, cancellationToken);
        }

        public Task<ByteContent> ReadHlsSegmentAsync(Guid id, Resolution resolution, int segment, CancellationToken cancellationToken = default)
        {
            return _readMethods.ReadHlsSegmentAsync(id, resolution, segment, cancellationToken);
        }

        public IEnumerable<int> GetHlsSegments(Guid id, Resolution resolution)
        {
            return _readMethods.GetHlsSegments(id, resolution);
        }

        public Task<ByteContent> ReadResizedAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default)
        {
            return _readMethods.ReadResizedAsync(id, resolution, cancellationToken);
        }

        public Task<ByteContent> ReadSelectedFrameAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _readMethods.ReadSelectedFrameAsync(id, cancellationToken);
        }

        public Task<ByteContent> ReadSourceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _readMethods.ReadSourceAsync(id, cancellationToken);
        }

        public Task SaveFrameAsync(Guid id, int index, ByteContent content, CancellationToken cancellationToken = default)
        {
            return _saveMethods.SaveFrameAsync(id, index, content, cancellationToken);
        }

        public Task SaveHlsListAsync(Guid id, Resolution resolution, ByteContent content, CancellationToken cancellationToken = default)
        {
            return _saveMethods.SaveHlsListAsync(id, resolution, content, cancellationToken);
        }

        public Task SaveHlsSegmentAsync(Guid id, Resolution resolution, int segment, ByteContent content, CancellationToken cancellationToken = default)
        {
            return _saveMethods.SaveHlsSegmentAsync(id, resolution, segment, content, cancellationToken);
        }

        public Task SaveResizedAsync(Guid id, Resolution resolution, ByteContent content, CancellationToken cancellationToken = default)
        {
            return _saveMethods.SaveResizedAsync(id, resolution, content, cancellationToken);
        }

        public Task SaveSelectedFrameAsync(Guid id, int index, CancellationToken cancellationToken = default)
        {
            return _saveMethods.SaveSelectedFrameAsync(id, index, cancellationToken);
        }

        public Task SaveSourceAsync(Guid id, ByteContent content, CancellationToken cancellationToken = default)
        {
            return _saveMethods.SaveSourceAsync(id, content, cancellationToken);
        }

        public IEnumerable<Resolution> GetResolutions(Guid id)
        {
            return _readMethods.GetResolutions(id);
        }

        public Task DeleteAllAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _deleteMethods.DeleteAllAsync(id, cancellationToken);
        }
    }
}