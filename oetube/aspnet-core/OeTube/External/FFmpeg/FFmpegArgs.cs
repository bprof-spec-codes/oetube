namespace OeTube.External.FFmpeg
{
    public record FFmpegArgs(string InputPath, string OutputPath) 
    { 
        public string? Arguments { get; init;}

        public override string ToString()
        {
            string arguments = string.IsNullOrWhiteSpace(Arguments) ? " " : $" {Arguments} ";
            return $"-i {InputPath}{arguments}{OutputPath}";
        }
    }
}