namespace OeTube.Domain.Infrastructure.FFmpeg.Infos
{
    public class AudioStreamInfo : StreamInfo
    {
        public int SampleRate { get; init; }
        public int Channels { get; init; }
    }
}