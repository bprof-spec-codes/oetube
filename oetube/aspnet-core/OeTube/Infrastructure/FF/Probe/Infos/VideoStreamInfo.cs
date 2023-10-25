using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.FF.Probe.Infos
{
    public class VideoStreamInfo : StreamInfo
    {
        public Resolution Resolution { get; init; } = Resolution.Zero;
        public double Framerate { get; init; }
        public string? PixelFormat { get; init; }
    }
}