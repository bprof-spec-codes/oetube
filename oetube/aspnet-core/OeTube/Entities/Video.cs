using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Domain.Entities;
using static Volo.Abp.Http.MimeTypes;

namespace OeTube.Entities
{
    public enum AccessType
    {
        Private, Public, Group
    }

    public class Video : AggregateRoot<Guid>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public TimeSpan Duration { get; private set; }
        public AccessType Access { get; private set; }
        public DateTime CreationTime { get; private set; }
        public Guid CreatorId { get; set; }
        public IList<Group> AccessGroups { get; private set; } //IReadOnlyList
        public bool IsDeleted { get; set; }
        public DateTime DeletionTime { get; set; }

        public Video(Guid id, string name, string description, TimeSpan duration,  DateTime creationTime, Guid creatorId)
        {
            Id = id;
            Name = name;
            Description = description;
            Duration = duration;
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

        public void SetAccess(AccessType access)
        {
            Access = access;
        }

        public void AddAccessGroup(Group group)
        {
            AccessGroups.Add(group);
        }

        public void RemoveAccessGroup(Guid groupId)
        {
            var item = AccessGroups.Where(x => x.Id == groupId).FirstOrDefault();
            if (item == null)
            {
                throw new ArgumentException("There is no Group with this Id: " + groupId);
            }
            AccessGroups.Remove(item);
        }


    }
}
