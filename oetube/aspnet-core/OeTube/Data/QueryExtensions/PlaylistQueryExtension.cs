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

            return context.GetAvaliableVideos(requesterId)
                          .OrderBy(p => p.Id)
                          .FirstOrDefault(p => p.Id == playlist.Id) is not null;
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
            playlists ??= context.Set<Playlist>();

            var videos = from videoItem in context.Set<VideoItem>()
                         join video in context.GetAvaliableVideos(requesterId)
                         on videoItem.VideoId equals video.Id
                         select videoItem;

            var result = (from playlist in playlists
                          join videoItem in videos
                          on playlist.Id equals videoItem.PlaylistId
                          select playlist).Distinct();
            return result;
        }
    }
}