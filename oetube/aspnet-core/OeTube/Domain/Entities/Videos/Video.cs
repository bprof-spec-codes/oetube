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
using Volo.Abp.Http.Modeling;

namespace OeTube.Domain.Entities.Videos
{


    public class Video : AggregateRoot<Guid>, IMayHaveCreator, IHasCreationTime,IHasAtomicKey<Guid>
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public TimeSpan? Duration { get; private set; }
        public AccessType Access { get; private set; }
        public DateTime CreationTime { get; private set; }
        public Guid? CreatorId { get; private set; }
        public VideoState State { get; private set; }
        private readonly EntitySet<AccessGroup,Guid> accessGroups;
        public virtual IReadOnlyEntitySet<AccessGroup, Guid> AccessGroups => accessGroups;

        Guid IHasAtomicKey<Guid>.AtomicKey => Id;

        private Video()
        {
            Name = string.Empty;
            accessGroups = new EntitySet<AccessGroup,Guid>();
        }

        public Video(Guid id, string name, Guid creatorId):this()
        {
            Id = id;
            SetName(name);
            CreationTime = DateTime.Now;
            CreatorId = creatorId;
            State = VideoState.Uploading;
        }

        public Video SetName(string name)
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

        public Video SetAccess(AccessType access)
        {
            Access = access;
            return this;
        }

        public Video SetStateToConverting()
        {
            if(State==VideoState.Ready)
            {
                throw new UserFriendlyException("The video is ready, you cannot set the state to converting!");
            }

            State = VideoState.Converting;
            return this;
        }

        public Video SetStateToReady()
        {
            if(State==VideoState.Ready)
            {
                throw new UserFriendlyException("The video is ready!");
            }
            if (Duration == null)
            {
                throw new UserFriendlyException("Duration is not set yet!");
            }

            State = VideoState.Ready;
            return this;
        }
        public Video SetDuration(TimeSpan duration)
        {
            if(Duration!=null)
            {
                throw new UserFriendlyException("Duration has already been set!");
            }
            Duration = duration;
            return this;
        }

    }
    public enum AccessType
    {
        Private, Public, Group
    }
    public enum VideoState
    {
        Uploading, Converting, Ready
    }
    public static class VideoConstants
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;

        public const int DescriptionMaxLength = 1000;
    }
}
