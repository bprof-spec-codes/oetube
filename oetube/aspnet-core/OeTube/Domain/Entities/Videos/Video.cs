using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Entities.Videos
{
    public class Video : AggregateRoot<Guid>, IMayHaveCreator, IHasCreationTime, IHasAtomicKey<Guid>, IHasName
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public AccessType Access { get; private set; }
        public DateTime CreationTime { get; private set; }
        public Guid? CreatorId { get; private set; }
        public TimeSpan Duration { get; private set; }
        public bool IsUploadCompleted { get; private set; }

        private readonly EntitySet<AccessGroup, Guid> accessGroups;
        public virtual IReadOnlyEntitySet<AccessGroup, Guid> AccessGroups => accessGroups;
        private readonly EntitySet<VideoResolution, Resolution> resolutions;
        public virtual IReadOnlyEntitySet<VideoResolution, Resolution> Resolutions => resolutions;
        Guid IHasAtomicKey<Guid>.AtomicKey => Id;

        private Video()
        {
            Name = string.Empty;
            accessGroups = new EntitySet<AccessGroup, Guid>();
            resolutions = new EntitySet<VideoResolution, Resolution>();
        }

        public Video(Guid id, string name, Guid? creatorId, TimeSpan duration, IEnumerable<Resolution> resolutions) : this()
        {
            Id = id;
            SetName(name);
            CreationTime = DateTime.Now;
            CreatorId = creatorId;
            Duration = duration;

            foreach (var item in resolutions)
            {
                this.resolutions.Add(new VideoResolution(Id, item));
            }
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

        public Video SetUploadCompleted()
        {
            if (IsUploadCompleted)
            {
                throw new InvalidOperationException();
            }
            if (!IsAllResolutionReady())
            {
                throw new InvalidOperationException();
            }
            IsUploadCompleted = true;
            return this;
        }

        public IEnumerable<Resolution> GetResolutionsBy(bool isReady)
        {
            return resolutions.Where(r => r.IsReady == isReady).Select(r => r.GetResolution());
        }

        public bool IsAllResolutionReady()
        {
            return resolutions.Count > 0 && resolutions.All(r => r.IsReady);
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
    }

    public static class VideoConstants
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 1000;
        public const int FormatMaxLength = 10;
        public const int FormatMinLength = 2;
    }
}