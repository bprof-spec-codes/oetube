namespace OeTube.Infrastructure.FFmpeg.Info
{
    public abstract class StreamInfo
    {
        public string Codec { get; init; }
        public TimeSpan Duration { get; init; }
        public long Bitrate { get; init; }
    }
}