namespace OeTube.Application.Caches.Source
{
    public class SourceCacheItem
    {
        public int CheckSum { get; }
        public SourceCacheItem(int checkSum)
        {
            CheckSum = checkSum;
        }

    }
}