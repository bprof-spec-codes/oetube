using OeTube.Entities;
using Volo.Abp.Domain.Entities;

namespace OeTube.Domain.Entities.Videos
{
    public class VideoResolution:Entity,IHasAtomicKey<Resolution>
    {
        public Guid VideoId { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsReady { get; private set; }
        Resolution IHasAtomicKey<Resolution>.AtomicKey => GetResolution();

        private VideoResolution()
        {
        }
        public VideoResolution(Guid videoId,Resolution resolution)
        {
            VideoId = videoId;
            if (resolution.Width <= 0)  
            {
                throw new ArgumentOutOfRangeException(nameof(resolution.Width), resolution.Width, null);
            }
            if (resolution.Height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(resolution.Height), resolution.Height, null);
            }
            Width =resolution.Width;
            Height = resolution.Height;
        }
        public Resolution GetResolution()
        {
            return new Resolution(Width, Height);
        }
        public void MarkReady()
        {
            if (IsReady)
            {
                throw new InvalidOperationException();
            }
            IsReady = true;
        }

        public override object[] GetKeys()
        {
            return new object[]{ VideoId, Width,Height }; 
        }
    }
}
