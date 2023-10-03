using Volo.Abp.Domain.Entities;


namespace OeTube.Entities
{
    public class Group : AggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreationTime { get; private set; }
        public Guid CreatorId { get; set; }
        private List<Member> _members;
        public IReadOnlyCollection<Member> Members 
        {
            get
            {
                return _members;
            }
        }
        public bool IsDeleted { get; set; }
        public DateTime DeletionTime { get; set; }

        public Group()
        {
            _members = new List<Member>();
        }

        public Group(Guid id, string name, string description, DateTime creationTime, Guid creatorId)
        {
            Id = id;
            Name = name;
            Description = description;
            CreationTime = creationTime;
            CreatorId = creatorId;
            _members = new List<Member>();
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void AddMember(Member member)
        {
            _members.Add(member);
        }

        public void RemoveMember(Member member)
        {
            if (!_members.Remove(member))
            {
                throw new ArgumentException("There is no Group with this Id: " + member.Id);
            }
        }
    }
}
