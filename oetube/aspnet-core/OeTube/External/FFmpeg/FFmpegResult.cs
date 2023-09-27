namespace OeTube.External.FFmpeg
{
    public class FFmpegResult
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        public TimeSpan Duration => End - Start;
        public string? Arguments { get; init; }
    }
}