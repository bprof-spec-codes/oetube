namespace OeTube.Application.Caches
{
    public class CacheItem<TValue>
    {
        public  TValue Value { get; set; }

        public CacheItem(TValue value)
        {
            Value = value;
        }
    }
  

}
