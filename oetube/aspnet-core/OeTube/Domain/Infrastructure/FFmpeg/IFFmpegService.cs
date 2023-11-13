namespace OeTube.Domain.Infrastructure.FFmpeg
{
    public interface IFFMpegService
    {
        Guid Id { get; }
        bool WriteToDebug { get; set; }

        Task CleanUpAsync(CancellationToken cancellationToken = default);

        Task<FFmpegResult> ExecuteAsync(ByteContent? input, string arguments, string? processName = null, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(string name, CancellationToken cancellationToken = default);

        Task<ByteContent> GetContentAsync(string name, CancellationToken cancellationToken = default);

        HashSet<string> GetFiles();
    }
}