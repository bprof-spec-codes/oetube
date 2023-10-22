using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.VideoStorages
{
    public interface IVideoStorageSave
    {
        Task FrameAsync(Guid id, int index, ByteContent content, CancellationToken cancellationToken = default);
        Task HlsListAsync(Guid id, Resolution resolution, ByteContent content, CancellationToken cancellationToken = default);
        Task HlsSegmentAsync(Guid id, Resolution resolution, int segment, ByteContent content, CancellationToken cancellationToken = default);
        Task ResizedAsync(Guid id, Resolution resolution, ByteContent content, CancellationToken cancellationToken = default);
        Task SelectedFrameAsync(Guid id, int index, CancellationToken cancellationToken = default);
        Task SourceAsync(Guid id, ByteContent content, CancellationToken cancellationToken = default);
    }
}