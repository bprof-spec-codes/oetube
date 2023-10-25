using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.Domain.Repositories;

namespace OeTube.Domain.Repositories
{
    public interface IVideoRepository : IRepository<Video, Guid>
    {
        Task<IQueryable<Video>> GetCompletedVideosQueryableAsync(string? name=null);
        Task<Video> UpdateAccessGroupsAsync(Video video, IEnumerable<Group> groups, bool autoSave = false, CancellationToken cancellationToken = default);
    }
}
