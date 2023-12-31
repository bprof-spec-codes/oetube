﻿using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Domain.Repositories
{
    public interface IPlaylistRepository :
        ICustomRepository<Playlist, Guid, IPlaylistQueryArgs>,
        IMayHaveCreatorRepository<Playlist, Guid, OeTubeUser>,
        IParentUpdateRepository<Playlist, Video>,
        IChildQueryAvaliableRepository<Playlist, Guid, Video, IVideoQueryArgs>,
        IHasAccessRepository<Playlist, Guid>,
        IQueryAvaliableRepository<Playlist, IPlaylistQueryArgs>
    {
        Task<int> GetAvaliableItemsCountAsync(Guid? requesterId, Playlist playlist, CancellationToken cancellationToken = default);
        Task<TimeSpan> GetAvaliableTotalDurationAsync(Guid? requesterId, Playlist playlist);
        Task<int> GetItemsCountAsync(Playlist playlist, CancellationToken cancellationToken = default);
        Task<TimeSpan> GetTotalDurationAsync(Playlist playlist);
    }
}