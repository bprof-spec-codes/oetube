using OeTube.Domain.Infrastructure.FFmpeg.Infos;

namespace OeTube.Domain.Infrastructure.FFmpeg
{
    public interface IFFProbeService
    {
        Guid Id { get; }

        Task<VideoInfo> AnalyzeAsync(ByteContent? content, CancellationToken cancellationToken = default);
    }
}