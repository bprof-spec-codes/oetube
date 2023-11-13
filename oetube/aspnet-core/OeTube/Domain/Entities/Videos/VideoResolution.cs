using Volo.Abp.Domain.Entities;
using Volo.Abp;
namespace OeTube.Domain.Entities.Videos
{
    public class VideoResolution : Entity, IHasAtomicKey<Resolution>
    {
        public Guid VideoId { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsReady { get; private set; }
        Resolution IHasAtomicKey<Resolution>.AtomicKey => GetResolution();

        private VideoResolution()
        {
        }

        public VideoResolution(Guid videoId, Resolution resolution)
        {
            VideoId = videoId;
            Check.Range(resolution.Width, nameof(resolution), 1);
            Check.Range(resolution.Height, nameof(resolution), 1);
            Width = resolution.Width;
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
            return new object[] { VideoId, Width, Height };
        }
    }
}