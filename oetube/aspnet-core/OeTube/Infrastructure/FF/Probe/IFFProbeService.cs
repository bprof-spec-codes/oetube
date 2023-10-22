using OeTube.Infrastructure.FF.Probe.Infos;
using Volo.Abp.Content;

namespace OeTube.Infrastructure.FF.Probe
{
    public interface IFFProbeService
    {
        Guid Id { get; }

        Task<VideoInfo> AnalyzeAsync(ByteContent? content, CancellationToken cancellationToken = default);
    }
}