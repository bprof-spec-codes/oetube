namespace OeTube.Infrastructure.FFprobe.Infos
{
    public abstract class StreamInfo
    {
        public string Codec { get; init; } = string.Empty;
        public TimeSpan Duration { get; init; }
        public long Bitrate { get; init; }
        public int Frames { get; init; }
    }
}