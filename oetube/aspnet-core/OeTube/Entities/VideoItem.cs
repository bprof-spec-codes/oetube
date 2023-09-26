using Volo.Abp.Domain.Entities;

namespace OeTube.Entities
{
    public class VideoItem : Entity
    {
        public Guid VideoId { get; private set; }
        public string AdditionalDescription { get; private set; }
        public int Order { get; set; }

        public VideoItem()
        {

        }

        public VideoItem(Guid videoId, int order)
        {
            VideoId = videoId;
            Order = order;
        }

        public void SetAdditionalDescription(string description)
        {
            AdditionalDescription = description;
        }

        public override object[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}
