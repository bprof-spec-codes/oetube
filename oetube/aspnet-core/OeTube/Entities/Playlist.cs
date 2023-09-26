using Volo.Abp.Domain.Entities;

namespace OeTube.Entities
{
    public class Playlist : AggregateRoot<Guid>
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public IList<Video> Items { get; private set; } // PlaylistItem vagy Video? 

        public DateTime CreationTime { get; private set; }

        public Guid CreatorId { get; private set; }

        public Playlist()
        {
        }

        public Playlist(Guid id, string name, string description, DateTime creationTime, Guid creatorId)
        {
            Id = id;
            Name = name;
            Description = description;
            CreationTime = creationTime;
            CreatorId = creatorId;
        }


        public void SetName(string name)
        {
            Name = name;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void AddItem(Video video)
        {
            Items.Add(video);
        }

        public void RemoveItem(Guid videoId)
        {
            var item = Items.Where(x => x.Id == videoId).FirstOrDefault();
            if (item == null)
            {
                throw new ArgumentException("There is no Video with this Id: " + videoId);
            }
            Items.Remove(item);
        }
    }
}
