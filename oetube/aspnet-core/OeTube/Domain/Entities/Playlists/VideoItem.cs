using OeTube.Domain.Entities.Videos;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Entities.Playlists
{
    public class VideoItem : Entity, IHasAtomicKey<int>,IHasForeignKey<Playlist,Guid>,IHasForeignKey<Video,Guid>
    {
        public Guid PlaylistId { get; private set; }
        public Guid VideoId { get; private set; }
        public int Order { get; private set; }

        int IHasAtomicKey<int>.AtomicKey => Order;

        Guid IHasForeignKey<Playlist, Guid>.ForeignKey => PlaylistId;

        Guid IHasForeignKey<Video, Guid>.ForeignKey => VideoId;

        private VideoItem()
        { }

        public VideoItem(Guid playlistId, int order, Guid videoId)
        {
            PlaylistId = playlistId;
            Check.Range(order, nameof(order), 0);
            Order = order;
            VideoId = videoId;
        }

        public override object[] GetKeys()
        {
            return new object[] { PlaylistId, Order };
        }
    }

    public static class VideoItemConstants
    {
        public const int VideoItemMinOrder = 0;
    }
}