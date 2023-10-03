using Volo.Abp.Domain.Entities;

namespace OeTube.Entities
{
    public class Member : Entity<Guid>
    {
        private Member() { }

        public Member(Guid id) : base(id) { }
    }
}
