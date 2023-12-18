using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;

namespace OeTube.Data.QueryExtensions
{

    public static class PlaylistQueryExtension
    {
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
            var result = from video in context.GetAvaliableVideos(requesterId)
                         join videoItem in context.Set<VideoItem>()
                         on video.Id equals videoItem.VideoId
                         where videoItem.PlaylistId == playlist.Id
                         orderby videoItem.Order
                         select video;
            return result;
        }

        public static IQueryable<Video> GetVideos(this OeTubeDbContext context, Playlist playlist)
        {
            var result = from video in context.Set<Video>()
                         join videoItem in context.Set<VideoItem>()
                         on video.Id equals videoItem.VideoId
                         where videoItem.PlaylistId == playlist.Id
                         orderby videoItem.Order
                         select video;
            return result;
        }

        public static IQueryable<Playlist> GetAvaliablePlaylists(this OeTubeDbContext context, Guid? requesterId, IQueryable<Playlist>? playlists = null)
        {
            playlists??= context.Set<Playlist>();
            var creator = from playlist in playlists
                          where playlist.CreatorId!=null&& playlist.CreatorId == requesterId
                          select playlist;

            var result = from playlist in playlists
                         join videoItem in context.Set<VideoItem>()
                         on playlist.Id equals videoItem.PlaylistId
                         join video in context.GetAvaliableVideos(requesterId)
                         on videoItem.VideoId equals video.Id
                         select playlist;

            return creator.Union(result).Distinct();
        }
    }
}