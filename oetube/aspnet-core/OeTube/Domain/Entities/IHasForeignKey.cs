using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Entities
{
    public interface IChildEntityReference<TParentKey,TReferencedKey>
    {
        public TParentKey ParentKey { get; }
        public TReferencedKey ReferencedKey { get; }
    }
 
}