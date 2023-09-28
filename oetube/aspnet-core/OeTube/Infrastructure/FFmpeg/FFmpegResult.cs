namespace OeTube.Infrastructure.FFmpeg
{
    public record FFmpegResult(FFmpegCommand Command, DateTime Start, DateTime End, string? Arguments)
    {
        public TimeSpan Duration => End - Start;
    }



}