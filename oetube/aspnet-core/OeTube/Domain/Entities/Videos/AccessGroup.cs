using OeTube.Domain.Entities.Groups;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Entities.Videos
{
    public class AccessGroup : Entity, IHasCreationTime, IHasAtomicKey<Guid>,IHasForeignKey<Group,Guid>,IHasForeignKey<Video,Guid>
    {
        public Guid VideoId { get; private set; }
        public Guid GroupId { get; private set; }
        public DateTime CreationTime { get; private set; }

        Guid IHasAtomicKey<Guid>.AtomicKey => GroupId;

        Guid IHasForeignKey<Group, Guid>.ForeignKey => GroupId;

        Guid IHasForeignKey<Video, Guid>.ForeignKey => VideoId;

        private AccessGroup()
        { }

        public AccessGroup(Guid videoId, Guid groupId)
        {
            VideoId = videoId;
            GroupId = groupId;
            CreationTime = DateTime.Now;
        }

        public override object[] GetKeys()
        {
            return new object[] { VideoId, GroupId };
        }
    }
}