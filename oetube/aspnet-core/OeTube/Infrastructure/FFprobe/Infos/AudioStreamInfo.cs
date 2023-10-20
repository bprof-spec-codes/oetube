namespace OeTube.Infrastructure.FFprobe.Infos
{
    public class AudioStreamInfo : StreamInfo
    {
        public int SampleRate { get; init; }
        public int Channels { get; init; }
    }
}