using OeTube.Domain.Entities.Videos;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;

namespace OeTube.Domain.Repositories.Queries
{
    public interface IAccessQuery
    {
        Task<IQueryable<Video>> GetAvaliableVideosAsync(OeTubeUser user);
        Task<bool> HasAccess(OeTubeUser user, Video video);
    }
}