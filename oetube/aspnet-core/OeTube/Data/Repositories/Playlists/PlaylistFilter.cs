using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Repositories.QueryArgs;
using System.Linq.Expressions;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Repositories.Playlists
{
    public class PlaylistFilter : Filter<Playlist, IPlaylistQueryArgs>, ITransientDependency
    {

        protected override Expression<Func<Playlist, bool>> GetFilter(IPlaylistQueryArgs args)
        {
            return playlist =>
                (string.IsNullOrWhiteSpace(args.Name) || playlist.Name.ToLower().Contains(args.Name.ToLower())) &&
                (args.CreationTimeMin == null || args.CreationTimeMin <= playlist.CreationTime) &&
                (args.CreationTimeMax == null || args.CreationTimeMax >= playlist.CreationTime);
        }
    }
}