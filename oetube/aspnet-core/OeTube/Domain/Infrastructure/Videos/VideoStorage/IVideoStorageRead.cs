using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.Videos;

namespace OeTube.Domain.Infrastructure.VideoStorage
{
    public interface IVideoStorageRead
    {
        Task<ByteContent> ReadFrameAsync(Guid id, int index, CancellationToken cancellationToken = default);

        IEnumerable<int> GetFrames(Guid id);

        Task<ByteContent> ReadHlsListAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default);

        Task<ByteContent> ReadHlsSegmentAsync(Guid id, Resolution resolution, int segment, CancellationToken cancellationToken = default);

        IEnumerable<int> GetHlsSegments(Guid id, Resolution resolution);

        Task<ByteContent> ReadResizedAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default);

        Task<ByteContent> ReadSelectedFrameAsync(Guid id, CancellationToken cancellationToken = default);

        Task<ByteContent> ReadSourceAsync(Guid id, CancellationToken cancellationToken = default);

        IEnumerable<Resolution> GetResolutions(Guid id);
    }
}