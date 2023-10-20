using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.FFprobe.Infos
{
    public class VideoStreamInfo : StreamInfo
    {
        public Resolution Resolution { get; init; }
        public double Framerate { get; init; }
        public string? PixelFormat { get; init; }
    }
}