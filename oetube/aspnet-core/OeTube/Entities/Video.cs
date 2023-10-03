using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using static Volo.Abp.Http.MimeTypes;

namespace OeTube.Entities
{
    public enum AccessType
    {
        Private, Public, Group
    }

    public class Video : AggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public TimeSpan Duration { get; private set; }
        public AccessType Access { get; private set; }
        public DateTime CreationTime { get; private set; }
        public Guid CreatorId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletionTime { get; set; }
        private List<AccessGroup> _accessGroups;
        public IReadOnlyCollection<AccessGroup> AccessGroups
        { 
            get
            {
                return _accessGroups;
            }
        }

        private Video()
        {
            _accessGroups = new List<AccessGroup>();
        }

        public Video(Guid id, string name, string description, TimeSpan duration,  DateTime creationTime, Guid creatorId)
        {
            Id = id;
            Name = name;
            Description = description;
            Duration = duration;
            CreationTime = creationTime;
            CreatorId = creatorId;
            _accessGroups = new List<AccessGroup>();
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

        public void AddAccessGroup(AccessGroup group)
        {
            _accessGroups.Add(group);
        }

        public void RemoveAccessGroup(AccessGroup group)
        {
            if (!_accessGroups.Remove(group))
            {
                throw new ArgumentException("There is no Group with this Id: " + group.Id);
            }
        }


    }
}
