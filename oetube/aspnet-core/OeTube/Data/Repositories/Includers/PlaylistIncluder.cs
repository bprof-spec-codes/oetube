using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Repositories.Extensions;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Repositories.Includers
{
    [ExposeServices(typeof(IIncluder<Playlist>))]
    public class PlaylistIncluder : Includer<Playlist>, ITransientDependency
    {
        protected override IEnumerable<string> GetNavigationProperties()
        {
           yield return nameof(Playlist.Items);
        }
    }

}
