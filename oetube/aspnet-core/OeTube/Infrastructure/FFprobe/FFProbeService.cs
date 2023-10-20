using OeTube.Infrastructure.FFprobe.Infos;
using OeTube.Infrastructure.FileContainers;
using OeTube.Infrastructure.ProcessTemplate;
using System.Net.Http;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFprobe
{

    public class FFProbeService : ITransientDependency, IFFProbeService
    {
        private readonly FFprobeProcess _ffprobe;
        private readonly IFileContainer _container;
        public Guid Id { get; }
        public FFProbeService(FFprobeProcess ffprobe, IFileContainerFactory containerFactory)
        {
            _ffprobe = ffprobe;
            _container = containerFactory.Create("ffprobe");
            Id = Guid.NewGuid();
        }
        public async Task<VideoInfo> AnalyzeAsync(ByteContent? input, CancellationToken cancellationToken = default)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            try
            {
              
                string fileName = Path.Combine(Id.ToString(), Path.GetFileName(input.Path));
                await _container.SaveAsync(input.WithNewPath(fileName), true, cancellationToken);
                var videoInfo = await _ffprobe.StartProcessAsync(new ProcessSettings(new NamedArguments(fileName), _container.RootDirectory), cancellationToken);
                await _container.DeleteAsync(fileName);
                return videoInfo;
            }
            finally
            {
                var directory=_container.GetAbsolutePath(Id.ToString());
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
            }
        }
    }
}
