namespace OeTube.Application.Caches
{
    public class CacheKey
    {
        public string Key { get; set; } = string.Empty;

        public override string ToString()
        {
            return Key;
        }
    }
}