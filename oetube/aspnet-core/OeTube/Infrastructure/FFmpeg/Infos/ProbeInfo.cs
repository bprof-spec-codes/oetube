namespace OeTube.Infrastructure.FFmpeg.Info
{
    public class ProbeInfo
    {
        public string FileName { get; init; }
        public TimeSpan Duration { get; init; }
        public long Size { get; init; }
        public IReadOnlyList<VideoInfo> VideoStreams { get; init; }
        public IReadOnlyList<AudioInfo> AudioStreams { get; init; }
    }
}