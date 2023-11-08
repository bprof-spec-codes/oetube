namespace OeTube.Application.Services.Caches.GroupCache
{
    public class GroupMembersCountCacheKey
    {
        public Guid GroupId { get; set; }
        public override string ToString()
        {
            return GroupId.ToString();
        }
    }
}
