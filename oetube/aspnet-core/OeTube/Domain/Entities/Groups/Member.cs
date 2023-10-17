using OeTube.Entities;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Entities.Groups
{
    public class Member : Entity, IHasCreationTime, IHasAtomicKey<Guid>
    {
        public Guid GroupId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime CreationTime { get; private set; }

        Guid IHasAtomicKey<Guid>.AtomicKey => UserId;

        private Member() { }

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
