using OeTube.Domain.Infrastructure.FFmpeg.Infos;
using OeTube.Domain.Infrastructure.Videos;

namespace OeTube.Domain.Infrastructure.FFmpeg
{
    public interface IFFProbeService
    {
        Guid Id { get; }

        Task<VideoInfo> AnalyzeAsync(ByteContent? content, CancellationToken cancellationToken = default);
    }
}