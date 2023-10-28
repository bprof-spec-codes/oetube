using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.Videos;

namespace OeTube.Domain.Infrastructure.VideoStorage
{
    public interface IVideoStorageSave
    {
        Task SaveFrameAsync(Guid id, int index, ByteContent content, CancellationToken cancellationToken = default);

        Task SaveHlsListAsync(Guid id, Resolution resolution, ByteContent content, CancellationToken cancellationToken = default);

        Task SaveHlsSegmentAsync(Guid id, Resolution resolution, int segment, ByteContent content, CancellationToken cancellationToken = default);

        Task SaveResizedAsync(Guid id, Resolution resolution, ByteContent content, CancellationToken cancellationToken = default);

        Task SaveSelectedFrameAsync(Guid id, int index, CancellationToken cancellationToken = default);

        Task SaveSourceAsync(Guid id, ByteContent content, CancellationToken cancellationToken = default);
    }
}