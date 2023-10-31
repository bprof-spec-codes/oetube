namespace OeTube.Domain.Infrastructure.FFmpeg.Infos
{
    public class VideoInfo
    {
        public string Format => Path.GetExtension(FileName).TrimStart('.');
        public string FileName { get; init; } = string.Empty;
        public TimeSpan Duration { get; init; }
        public long Size { get; init; }
        public IReadOnlyList<VideoStreamInfo> VideoStreams { get; init; } = Array.Empty<VideoStreamInfo>();
        public IReadOnlyList<AudioStreamInfo> AudioStreams { get; init; } = Array.Empty<AudioStreamInfo>();
    }
}