using Volo.Abp.Domain.Entities;


namespace OeTube.Entities
{
    public class Group : AggregateRoot<Guid>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreationTime { get; private set; }
        public Guid CreatorId { get; set; }
        public IList<User> Members { get; private set; } //IReadOnlyList
        public bool IsDeleted { get; set; }
        public DateTime DeletionTime { get; set; }

        public Group()
        {
            
        }

        public Group(Guid id, string name, string description, DateTime creationTime, Guid creatorId)
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

        public void AddMember(User user)
        {
            Members.Add(user);
        }

        public void RemoveMember(Guid userId)
        {
            var user = Members.Where(x => x.Id == userId).FirstOrDefault();
            if (user == null)
            {
                throw new ArgumentException("There is no User with this Id: " + userId);
            }
            Members.Remove(user);
        }
    }
}
