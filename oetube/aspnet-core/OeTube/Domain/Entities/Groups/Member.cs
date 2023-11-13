using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Entities.Groups
{
    public class Member : Entity, IHasCreationTime,IHasAtomicKey<Guid>, IChildEntityReference<Guid,Guid>
    {
        public Guid GroupId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime CreationTime { get; private set; }

        Guid IHasAtomicKey<Guid>.AtomicKey => UserId;

        Guid IChildEntityReference<Guid, Guid>.ParentKey => GroupId;

        Guid IChildEntityReference<Guid, Guid>.ReferencedKey => UserId;

        private Member()
        { }

        public Member(Guid groupId, Guid userId)
        {
            GroupId = groupId;
            UserId = userId;
            CreationTime = DateTime.Now;
        }

        public override object[] GetKeys()
        {
            return new object[] { GroupId, UserId };
        }
    }
}