using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;

namespace OeTube.Domain.Repositories.Queries
{
    public interface ICreatorQuery
    {
        Task<IQueryable<Group>> GetCreatedGroupsAsync(OeTubeUser user);
        Task<IQueryable<Playlist>> GetCreatedPlaylistsAsync(OeTubeUser user);
        Task<IQueryable<Video>> GetCreatedVideosAsync(OeTubeUser user);
    }
}