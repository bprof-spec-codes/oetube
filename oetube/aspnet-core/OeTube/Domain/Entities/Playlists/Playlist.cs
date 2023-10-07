using JetBrains.Annotations;
using Microsoft.Identity.Client;
using OeTube.Entities;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Entities.Playlists
{
    public class Playlist : AggregateRoot<Guid>, IHasCreationTime, IMayHaveCreator
    {
        public string Name { get; private set; }

        public string? Description { get; private set; }

        private EntitySet<VideoItem, int> items;

        public virtual IReadOnlyEntitySet<VideoItem,int> Items => items;

        public DateTime CreationTime { get; private set; }

        public Guid? CreatorId { get; private set; }

        public Playlist()
        {
            items = new EntitySet<VideoItem,int>();
        }

        public Playlist(Guid id,[NotNull]string name, Guid creatorId)
        {
            Id = id;
            SetName(name);
            CreationTime = DateTime.Now;
            CreatorId = creatorId;
            items = new EntitySet<VideoItem,int>();
        }


        public Playlist SetName([NotNull]string name)
        {
            Check.Length(name,
                         nameof(name),
                         PlaylistConstants.NameMaxLength,
                         PlaylistConstants.NameMinLength);
            Name = name;
            return this;
        }

        public Playlist SetDescription(string? description)
        {
            Check.Length(description, nameof(description),
                         PlaylistConstants.DescriptionMaxLength);
            Description = description;
            return this;
        }

        public Playlist AddItem(int order, Guid videoId)
        {
            if (!items.Add(new VideoItem(Id,order,videoId)))
            {
                throw new ArgumentException();
            }
            return this;
        }

        public Playlist RemoveItem(int order)
        {
            if (!items.Remove(new VideoItem(Id, order, Guid.Empty)))
            {
                throw new ArgumentException();
            }
            return this;
        }
    }
    public static class PlaylistConstants
    {
        public const int NameMinLength = 3; 
        public const int NameMaxLength = 100;

        public const int DescriptionMaxLength = 1000;
    }
}
