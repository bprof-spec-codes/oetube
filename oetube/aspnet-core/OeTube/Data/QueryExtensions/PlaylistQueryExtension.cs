using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;

namespace OeTube.Data.QueryExtensions
{
    public class PlaylistAccessibility
    {
        public OeTubeUser? User { get; set; }
        public Playlist? Playlist { get; set; }
    }
    public static class PlaylistQueryExtension
    {
        public static IQueryable<PlaylistAccessibility> GetPlaylistAccessibilities(this OeTubeDbContext context,IQueryable<Playlist>? playlists=null)
        {
            playlists??= context.Set<Playlist>();

            var result= from playlist in playlists
                        join videoItem in context.Set<VideoItem>()
                        on playlist.Id equals videoItem.PlaylistId
                        join accessibility in context.GetVideoAccessibilities()
                        on videoItem.VideoId equals accessibility.Video!.Id
                        select new PlaylistAccessibility()
                        {
                            Playlist = playlist,
                            User = accessibility.User
                        };

            return result.Distinct();
                            
        }
        public static bool HasAccess(this OeTubeDbContext context, Guid? requesterId, Playlist playlist)
        {
            if (playlist.CreatorId == requesterId)
            {
                return true;
            }

            return context.GetAvaliableVideos(requesterId, playlist).Any();
        }
        
        public static IQueryable<Video> GetAvaliableVideos(this OeTubeDbContext context, Guid? requesterId, Playlist playlist)
        {
            return context.GetAvaliableVideos(requesterId, GetVideos(context, playlist));
        }

        public static IQueryable<Video> GetVideos(this OeTubeDbContext context, Playlist playlist)
        {
            var result = from videoItem in context.Set<VideoItem>()
                         where videoItem.PlaylistId == playlist.Id
                         join video in context.Set<Video>()
                         on videoItem.VideoId equals video.Id
                         orderby videoItem.Order
                         select video;
            return result;
        }

        public static IQueryable<Playlist> GetAvaliablePlaylists(this OeTubeDbContext context, Guid? requesterId, IQueryable<Playlist>? playlists = null)
        {
            var result = from accessibility in context.GetPlaylistAccessibilities(playlists)
                         where accessibility.User!.Id == requesterId
                         select accessibility.Playlist;

            return result;
        }
    }
}