using OeTube.Domain.Entities.Playlists;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Repositories.Playlists
{
    public class PlaylistIncluder : Includer<Playlist>, ITransientDependency
    {
        protected override IEnumerable<string> GetNavigationProperties()
        {
            yield return nameof(Playlist.Items);
        }
    }
}