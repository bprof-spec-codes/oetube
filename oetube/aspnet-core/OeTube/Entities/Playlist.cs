using Volo.Abp.Domain.Entities;

namespace OeTube.Entities
{
    public class Playlist : AggregateRoot<Guid>
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        private List<VideoItem> _items { get; set; }

        public IReadOnlyCollection<VideoItem> Items
        {
            get
            {
                return _items;
            }
        }

        public DateTime CreationTime { get; private set; }

        public Guid CreatorId { get; private set; }

        public Playlist()
        {
            _items = new List<VideoItem>();
        }

        public Playlist(Guid id, string name, string description, DateTime creationTime, Guid creatorId)
        {
            Id = id;
            Name = name;
            Description = description;
            CreationTime = creationTime;
            CreatorId = creatorId;
            _items = new List<VideoItem>();
        }


        public void SetName(string name)
        {
            Name = name;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void AddItem(VideoItem item)
        {
            _items.Add(item);
        }

        public void RemoveItem(VideoItem item)
        {
            if (!_items.Remove(item))
            {
                throw new ArgumentException("There is no Group with this Id: " + item.Id);
            }
        }
    }
}
