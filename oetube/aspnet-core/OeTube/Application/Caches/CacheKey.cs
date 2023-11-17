namespace OeTube.Application.Caches
{
    public class CacheKey
    {
        public CacheKey(string key)
        {
            Key = key;
        }
        public string Key { get; set; }
        public override string ToString()
        {
            return Key;
        }
    }
}