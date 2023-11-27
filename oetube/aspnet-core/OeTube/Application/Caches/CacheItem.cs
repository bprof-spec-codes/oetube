namespace OeTube.Application.Caches
{
    public class CacheItem<TValue>
    {
        public int? SourceCheckSum { get; set; }
        public TValue? Value { get; set; }

        public CacheItem(TValue? value, int? sourceCheckSum)
        {
            Value = value;
            SourceCheckSum = sourceCheckSum;
        }
    }
}