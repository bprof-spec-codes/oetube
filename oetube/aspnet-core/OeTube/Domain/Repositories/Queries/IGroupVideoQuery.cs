using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.Application.Dtos;

namespace OeTube.Data.Queries
{

    public interface IGroupVideoQuery
    {
        Task<IQueryable<AccessGroup>> GetAccessGroupsAsync();
        Task<IQueryable<Group>> GetAccessGroupsAsync(Video video);
        Task<IQueryable<Video>> GetAvaliableVideosAsync(Group group);
    }
}
