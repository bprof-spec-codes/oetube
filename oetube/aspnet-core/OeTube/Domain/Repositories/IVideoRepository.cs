using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Domain.Repositories
{
    public interface IVideoRepository :
        ICustomRepository<Video, Guid, IVideoQueryArgs>,
        IMayHaveCreatorRepository<Video, Guid, OeTubeUser>,
        IParentUpdateRepository<Video,Group>,
        IChildQueryRepository<Video,Guid,Group,IGroupQueryArgs>,
        IQueryAvaliableRepository<Video,  IVideoQueryArgs>,
        IHasAccessRepository<Video,Guid>
    {
        Task<PaginationResult<Video>> GetUncompletedVideosAsync(TimeSpan old = default, IQueryArgs? args = null, bool includeDetails = false, CancellationToken cancellationToken = default);
    }
}