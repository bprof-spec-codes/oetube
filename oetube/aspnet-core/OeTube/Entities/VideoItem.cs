using Volo.Abp.Domain.Entities;

namespace OeTube.Entities
{
    public class VideoItem : Entity<int>
    {
        private VideoItem() { }

        public VideoItem(int id) : base(id) { }
    }
}
