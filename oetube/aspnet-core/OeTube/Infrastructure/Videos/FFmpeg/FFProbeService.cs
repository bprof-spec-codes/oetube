using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Infrastructure.FileContainers;
using OeTube.Infrastructure.ProcessTemplate;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.Videos.FFmpeg
{
    public class FFProbeService : ITransientDependency, IFFProbeService
    {
        private readonly FFprobeProcess _ffprobe;
        private readonly IFileContainer _container;
        public Guid Id { get; }

        public FFProbeService(FFprobeProcess ffprobe, IFileContainerFactory containerFactory)
        {
            Id = Guid.NewGuid();
            _ffprobe = ffprobe;
            _container = containerFactory.Create(Path.Combine("ffprobe", Id.ToString()));
        }

        public async Task<VideoInfo> AnalyzeAsync(ByteContent? input, CancellationToken cancellationToken = default)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            try
            {
                input = input.WithNewName("input");
                await _container.SaveAsync(input, true, cancellationToken);
                var videoInfo = await _ffprobe.StartProcessAsync(new ProcessSettings(new NamedArguments(input.Path), _container.RootDirectory), cancellationToken);
                await _container.DeleteAsync(input.Path);
                return videoInfo;
            }
            finally
            {
                if (Directory.Exists(_container.RootDirectory))
                {
                    Directory.Delete(_container.RootDirectory, true);
                }
            }
        }
    }
}