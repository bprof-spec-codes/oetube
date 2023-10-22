using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.VideoStorages
{
    public interface IVideoStorageDelete
    {
        void DeleteAll(Guid id);
        Task<bool> FrameAsync(Guid id, int index, CancellationToken cancellationToken = default);
        Task<bool> HlsListAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default);
        Task<bool> HlsSegmentAsync(Guid id, Resolution resolution, int segment, CancellationToken cancellationToken = default);
        Task<bool> ResizedAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default);
        Task<bool> SelectedFrameAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> SourceAsync(Guid id, CancellationToken cancellationToken = default);
    }
}