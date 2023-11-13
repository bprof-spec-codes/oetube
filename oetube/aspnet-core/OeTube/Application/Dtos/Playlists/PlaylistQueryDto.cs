using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Application.Dtos.Playlists
{
    public class PlaylistQueryDto : QueryDto, IPlaylistQueryArgs
    {
        public string? Name {get; set; }
        public DateTime? CreationTimeMin {get; set; }
        public DateTime? CreationTimeMax {get; set; }
        public Guid? CreatorId {get; set; }
    }
}
