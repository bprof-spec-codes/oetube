namespace OeTube.Infrastructure.FFmpeg.Info
{
    public class AudioInfo : StreamInfo
    {
        public int SampleRate { get; init; }
        public int Channels { get; init; }
    }
}