namespace OeTube.External.FFmpeg
{
    public class ProbeInfo
    {
        public string Path { get; init; }
        public TimeSpan Duration { get; init; }
        public DateTime? CreationTime { get; init; }
        public long Size { get; init; }
        public IReadOnlyList<VideoInfo> VideoStreams { get; init; }
        public IReadOnlyList<AudioInfo> AudioStreams { get; init; }
    }
}