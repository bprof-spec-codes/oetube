namespace OeTube.External.FFmpeg
{
    public class VideoInfo:StreamInfo
    {
        public int Width { get; init; }
        public int Height { get; init; }
        public double Framerate { get; init; }
        public string? Ratio { get; init; }
        public string? PixelFormat { get; init; }
    }
}