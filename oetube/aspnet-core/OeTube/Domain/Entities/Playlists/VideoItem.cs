﻿using OeTube.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp;

namespace OeTube.Domain.Entities.Playlists
{
    public class VideoItem : Entity, IHasLocalKey<int>
    {
        public Guid PlaylistId { get; private set; }
        public Guid VideoId { get; private set; }
        public int Order { get; private set; }

        int IHasLocalKey<int>.LocalKey => Order;

        private VideoItem() { }

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
