namespace OeTube.External.FFmpeg
{
    public class AudioInfo : StreamInfo
    {
        public int SampleRate { get; init; }
        public int Channels { get; init; }
        public string? Language { get; init; }
        public string? Title { get; init; }
    }
}