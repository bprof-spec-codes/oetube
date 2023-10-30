using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Infrastructure.ProcessTemplate;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFmpeg
{
    public class FFProbeService : ITransientDependency, IFFProbeService
    {
        private readonly FFprobeProcess _ffprobe;
        private readonly IFileContainer _container;
        public Guid Id { get; }
        public string RootDirectory => Path.Combine(_container.RootDirectory, Id.ToString());
        public FFProbeService(FFprobeProcess ffprobe, IFileContainerFactory containerFactory)
        {
            Id = Guid.NewGuid();
            _ffprobe = ffprobe;
            _container = containerFactory.Create<FFProbeService>();
        }

        public async Task<VideoInfo> AnalyzeAsync(ByteContent? input, CancellationToken cancellationToken = default)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            string name = "input." + input.Format;
            await _container.SaveAsync(new SimpleFileClass(Id,name), input, cancellationToken);
            var videoInfo = await _ffprobe.StartProcessAsync(new ProcessSettings(new NamedArguments(name),RootDirectory), cancellationToken);
            await _container.DeleteKeyFilesAsync(Id, cancellationToken);
            return videoInfo;

        }
    }
}