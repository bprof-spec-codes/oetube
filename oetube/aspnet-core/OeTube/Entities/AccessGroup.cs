using Volo.Abp.Domain.Entities;

namespace OeTube.Entities
{
    public class AccessGroup : Entity<Guid>
    {
        private AccessGroup() { }

        public AccessGroup(Guid id) : base(id) { }
    }
}
