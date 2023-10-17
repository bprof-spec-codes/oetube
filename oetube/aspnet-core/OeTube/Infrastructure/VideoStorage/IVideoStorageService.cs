using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace OeTube.Infrastructure.VideoStorage
{
    public interface IVideoStorageService
    {
        IEnumerable<string> GetSupportedFormats();
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Stream> GetIndexImageAsync(Guid id, CancellationToken cancellationToken = default);
        Task<int> GetIndexImageCountAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<int>> GetResolutionsAsnyc(Guid id, CancellationToken cancellationToken = default);
        Task<FileResult> GetVideoAsync(Guid id, int resolution, CancellationToken cancellationToken = default);
        Task SaveVideoAsync(Guid id, IRemoteStreamContent content, bool overrideExisting = true, CancellationToken cancellationToken = default);
        Task SetIndexImageAsync(Guid id, int index, CancellationToken cancellationToken = default);
        Task<FileStreamResult> GetM3U8Async(Guid id, int resolution,CancellationToken cancellationToken = default);
        Task<FileStreamResult> GetM3U8SegmentAsync(Guid id, int resolution, int segment, CancellationToken cancellationToken = default);
    }
}
