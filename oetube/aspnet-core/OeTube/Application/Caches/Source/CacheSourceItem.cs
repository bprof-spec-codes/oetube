namespace OeTube.Application.Caches.Source
{
    public class CacheSourceItem
    {
        public int CheckSum { get; }
        public CacheSourceItem(int checkSum)
        {
            CheckSum = checkSum;
        }

    }
}