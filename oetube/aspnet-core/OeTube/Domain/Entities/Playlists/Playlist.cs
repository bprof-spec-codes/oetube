using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Entities.Playlists
{
    public class Playlist : AggregateRoot<Guid>, IHasCreationTime, IMayHaveCreator, IHasAtomicKey<Guid>, IHasName
    {
        public string Name { get; private set; }

        public string? Description { get; private set; }

        private EntitySet<VideoItem, int> items;

        public virtual IReadOnlyEntitySet<VideoItem, int> Items => items;

        public DateTime CreationTime { get; private set; }

        public Guid? CreatorId { get; private set; }

        Guid IHasAtomicKey<Guid>.AtomicKey => Id;


        public Playlist()
        {
            items = new EntitySet<VideoItem, int>();
            Name = string.Empty;
        }

        public Playlist(Guid id, string name, Guid creatorId) : this()
        {
            Id = id;
            SetName(name);
            CreationTime = DateTime.Now;
            CreatorId = creatorId;
        }

        public Playlist SetName(string name)
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
    }

    public static class PlaylistConstants
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;

        public const int DescriptionMaxLength = 1000;
    }
}