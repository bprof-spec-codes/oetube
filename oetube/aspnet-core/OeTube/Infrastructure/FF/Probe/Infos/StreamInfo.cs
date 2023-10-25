namespace OeTube.Infrastructure.FF.Probe.Infos
{
    public abstract class StreamInfo
    {
        public string Codec { get; init; } = string.Empty;
        public TimeSpan Duration { get; init; }
        public long Bitrate { get; init; }
        public int Frames { get; init; }
    }
}