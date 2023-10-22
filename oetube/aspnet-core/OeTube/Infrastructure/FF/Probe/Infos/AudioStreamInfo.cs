namespace OeTube.Infrastructure.FF.Probe.Infos
{
    public class AudioStreamInfo : StreamInfo
    {
        public int SampleRate { get; init; }
        public int Channels { get; init; }
    }
}