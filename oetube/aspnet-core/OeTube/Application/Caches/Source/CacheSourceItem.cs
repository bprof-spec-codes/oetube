namespace OeTube.Application.Caches.Source
{
    public class CacheSourceItem
    {
        public int CheckSum { get; }
        public CacheSourceItem()
        {
            CheckSum = DateTime.Now.GetHashCode();
        }

    }
}