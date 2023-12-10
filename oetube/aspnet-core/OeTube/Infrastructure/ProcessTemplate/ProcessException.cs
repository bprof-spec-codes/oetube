namespace OeTube.Infrastructure.ProcessTemplate
{
    public class ProcessException : Exception
    {
        public string? StandardOutput { get; }
        public string? StandardError { get; }
        public int? ErrorCode { get; }

        public ProcessException(string message, int? errorCode = null, string? standardOutput = null, string? standardError = null) : base(message)
        {
            StandardOutput = standardOutput;
            StandardError = standardError;
            ErrorCode = errorCode;
        }
    }
}