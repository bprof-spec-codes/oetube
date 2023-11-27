using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Entities.Videos
{
    public class AccessGroup : Entity, IHasCreationTime, IHasAtomicKey<Guid>, IChildEntityReference<Guid, Guid>
    {
        public Guid VideoId { get; private set; }
        public Guid GroupId { get; private set; }
        public DateTime CreationTime { get; private set; }

        Guid IHasAtomicKey<Guid>.AtomicKey => GroupId;

        Guid IChildEntityReference<Guid, Guid>.ParentKey => VideoId;

        Guid IChildEntityReference<Guid, Guid>.ReferencedKey => GroupId;

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