using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Domain.Repositories
{
    public interface IQueryVideoRepository : IQueryRepository<Video, IVideoQueryArgs>
    {
        Task<List<Group>> GetAccessGroupsAsync(Video video, IGroupQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<List<Video>> GetUncompletedVideosAsync(TimeSpan? old = null, IVideoQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default);
    }

    public interface IUpdateVideoRepository : IUpdateRepository<Video, Guid>
    {
        Task<Video> UpdateAccessGroupsAsync(Video video, IEnumerable<Guid> groupIds, bool autoSave = false, CancellationToken cancellationToken = default);
    }

    public interface IVideoRepository : ICustomRepository<Video, Guid, IVideoQueryArgs>, IQueryVideoRepository, IUpdateVideoRepository
    {
    }
}