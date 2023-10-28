using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Infrastructure.VideoStorage
{
    public interface IVideoStorageDelete
    {
        Task DeleteAllAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> DeleteResizedAsync(Guid id, Resolution resolution, CancellationToken cancellationToken = default);

        Task<bool> DeleteSourceAsync(Guid id, CancellationToken cancellationToken = default);
    }
}