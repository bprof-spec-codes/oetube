namespace OeTube.Infrastructure.ProcessTemplate
{
    public record ProcessResult(ProcessSettings Settings, DateTime Start, DateTime End)
    {
        public TimeSpan Duration => End - Start;
    }
}