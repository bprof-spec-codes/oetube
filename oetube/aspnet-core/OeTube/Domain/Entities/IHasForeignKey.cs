using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Entities
{
    public interface IHasForeignKey<TReferencedEntity,TKey>
        where TReferencedEntity:IEntity<TKey>
    {
        public TKey ForeignKey { get; }
    }
    public interface IHasName
    {
        public string Name { get; }
    }
}