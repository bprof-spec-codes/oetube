using JetBrains.Annotations;
using OeTube.Domain.Entities.Playlists;
using OeTube.Entities;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace OeTube.Domain.Entities.Videos
{
   
    public class Video : AggregateRoot<Guid>, ISoftDelete, IMayHaveCreator, IHasCreationTime
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public TimeSpan? Duration { get; private set; }
        public AccessType Access { get; private set; }
        public DateTime CreationTime { get; private set; }
        public Guid? CreatorId { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime DeletionTime { get; set; }
        private readonly EntitySet<AccessGroup,Guid> accessGroups;
        public virtual IReadOnlyEntitySet<AccessGroup, Guid> AccessGroups => accessGroups;

        private Video()
        {
            accessGroups = new EntitySet<AccessGroup,Guid>();
        }

        public Video(Guid id, string name, Guid creatorId)
        {
            Id = id;
            SetName(name);
            CreationTime = DateTime.Now;
            CreatorId = creatorId;
            accessGroups = new EntitySet<AccessGroup,Guid>();
        }

        public Video SetName([NotNull]string name)
        {
            Check.Length(name,
                        nameof(name),
                        VideoConstants.NameMaxLength,
                        VideoConstants.NameMinLength);
            Name = name;
            return this;
        }

        public Video SetDescription(string? description)
        {
            Check.Length(description,
                         nameof(description),
                         VideoConstants.DescriptionMaxLength); 
            Description = description;
            return this;
        }
        public Video SetDuration(TimeSpan duration)
        {
            Duration = duration;
            return this;
        }
        public Video SetAccess(AccessType access)
        {
            Access = access;
            return this;
        }

        public Video AddAccessGroup(Guid groupId)
        {
            if(accessGroups.Add(new AccessGroup(Id,groupId)))
            {
                throw new ArgumentException();
            }
            return this;
        }

        public Video RemoveAccessGroup(Guid groupId)
        {
            if (!accessGroups.Remove(new AccessGroup(Id, groupId)))
            {
                throw new ArgumentException();
            }
            return this;
        }


    }
    public enum AccessType
    {
        Private, Public, Group
    }
    public static class VideoConstants
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;

        public const int DescriptionMaxLength = 1000;
    }
}
