namespace OeTube.Application.Services.Caches.VideoAccess
{
    public class VideoAccessCacheKey
    {
        public Guid? RequesterId { get; set; }
        public Guid VideoId { get; set; }
        public override string ToString()
        {
            return $"{RequesterId}_{VideoId}";
        }
    }
}
