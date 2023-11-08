namespace OeTube.Application.Services.Caches.GroupCache
{
    public class IsMemberCacheKey
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public override string ToString()
        {
            return $"{UserId}_{GroupId}";
        }
    }

}
