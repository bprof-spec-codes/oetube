namespace OeTube.External.FFmpeg
{
    public abstract class StreamInfo
    {
        public string Codec { get; init; }
        public TimeSpan Duration { get; init; }
        public long Bitrate { get; init; }
    }
}