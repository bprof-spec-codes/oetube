namespace OeTube.Domain.Entities
{
    public interface IHasAtomicKey<TKey>
        where TKey : notnull
    {
        public TKey AtomicKey { get; }
    }
}