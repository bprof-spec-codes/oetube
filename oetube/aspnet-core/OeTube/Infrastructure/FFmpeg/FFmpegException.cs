namespace OeTube.Infrastructure.FFmpeg
{
    public class FFmpegException : Exception
    {
        public string? StandardOutput { get; }
        public string? StandardError { get; }
        public FFmpegException(string message, string? standardOutput=null,string? standardError=null) : base(message) 
        {
            StandardOutput = standardOutput;
            StandardError = standardError;
        }
    }
}