using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.VideoStorages
{
    public interface IVideoStorageGet
    {
        Task<ByteContent> FrameAsync(Guid id, int index, CancellationToken cancellationToken = default);
        IEnumerable<int> Frames(Guid id);
        Task<ByteContent> HlsListAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default);
        Task<ByteContent> HlsSegmentAsync(Guid id, Resolution resolution, int segment, CancellationToken cancellationToken = default);
        IEnumerable<int> HlsSegments(Guid id, Resolution resolution);
        Task<ByteContent> ResizedAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default);
        Task<ByteContent> SelectedFrameAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ByteContent> SourceAsync(Guid id, CancellationToken cancellationToken = default);
    }
}