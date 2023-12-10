using OeTube.Domain.FilePaths;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Infrastructure.ProcessTemplate;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFMpeg
{
    public class FFMpegService : IFFMpegService, ITransientDependency
    {
        private readonly IFileContainer _container;
        private readonly FFMpegProcess _ffmpeg;
        public Guid Id { get; }
        public bool WriteToDebug { get; set; }
        public string RootDirectory => Path.Combine(_container.RootDirectory, Id.ToString());

        public FFMpegService(FFMpegProcess ffmpeg, IFileContainerFactory containerFactory)
        {
            Id = Guid.NewGuid();
            _ffmpeg = ffmpeg;
            _container = containerFactory.Create<FFMpegService>();
        }

        public async Task<bool> DeleteAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _container.DeleteFileAsync(new CustomFilePath(Id, name), cancellationToken);
        }

        public async Task<ByteContent> GetContentAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _container.GetFileAsync(new CustomFilePath(Id, name), cancellationToken);
        }

        public HashSet<string> GetFiles()
        {
            return _container.GetFiles(Id).ToHashSet();
        }

        public async Task<FFmpegResult> ExecuteAsync(ByteContent? input, string arguments, string? processName = null, CancellationToken cancellationToken = default)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            string name = "input." + input.Format;
            arguments = $"-i {name} {arguments}";

            var path = new CustomFilePath(Id, name);
            await _container.SaveFileAsync(path, input, cancellationToken);

            var settings = new ProcessSettings(
                new NamedArguments(arguments, processName ?? nameof(FFMpegService)),
                RootDirectory,
                WriteToDebug);

            var files = GetFiles();
            var result = await _ffmpeg.StartProcessAsync(settings, cancellationToken);
            var outputFiles = GetFiles();
            outputFiles.ExceptWith(files);
            return new FFmpegResult(result, outputFiles);
        }

        public async Task CleanUpAsync(CancellationToken cancellationToken = default)
        {
            await _container.DeleteKeyFilesAsync(Id, cancellationToken);
        }
    }
}