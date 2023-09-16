using Volo.Abp.Domain.Entities;

namespace OeTube.Entities
{
    public class Example:BasicAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
