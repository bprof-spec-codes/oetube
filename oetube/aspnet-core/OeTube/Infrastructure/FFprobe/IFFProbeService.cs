using OeTube.Infrastructure.FFprobe.Infos;
using Volo.Abp.Content;

namespace OeTube.Infrastructure.FFprobe
{
    public interface IFFProbeService
    {
        Guid Id { get; }

        Task<VideoInfo> AnalyzeAsync(ByteContent? content, CancellationToken cancellationToken = default);
    }
}