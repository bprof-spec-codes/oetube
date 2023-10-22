using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OeTube.Infrastructure.FileContainers;
using OeTube.Infrastructure.ProcessTemplate;
using OeTube.Infrastructure.VideoStorages;
using System.Runtime.CompilerServices;
using System.Threading;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FF.Mpeg
{
    public class FFmpegService : IFFmpegService, ITransientDependency
    {
        private readonly IFileContainer _container;
        private readonly FFmpegProcess _ffmpeg;
        public Guid Id { get; }
        public bool WriteToDebug { get; set; }
        public FFmpegService(FFmpegProcess ffmpeg, IFileContainerFactory containerFactory)
        {
            Id = Guid.NewGuid();
            _ffmpeg = ffmpeg;
            _container = containerFactory.Create(Path.Combine("ffmpeg", Id.ToString()));
        }
        public async Task<bool> DeleteAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _container.DeleteAsync(name, cancellationToken);
        }
        public async Task<ByteContent> GetContentAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _container.GetContentAsync(name, cancellationToken);
        }

        public HashSet<string> GetFiles()
        {
            var files = Directory.GetFiles(_container.RootDirectory, "*.*", SearchOption.AllDirectories);
            return files.Select(_container.GetContainerPath).ToHashSet();
        }
        public async Task<FFmpegResult> ExecuteAsync(ByteContent? input, string arguments, string? processName = null, CancellationToken cancellationToken = default)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            arguments = $"-i {input.Path} {arguments}";

            await _container.SaveAsync(input, true, cancellationToken);

            var settings = new ProcessSettings(
                new NamedArguments(arguments, processName ?? input.Path),
                _container.RootDirectory,
                WriteToDebug);

            var files = GetFiles();
            var result = await _ffmpeg.StartProcessAsync(settings, cancellationToken);
            var outputFiles = GetFiles();
            outputFiles.ExceptWith(files);
            return new FFmpegResult(result, outputFiles);
        }
        public async Task CleanUpAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var files = GetFiles();
                foreach (var item in files)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    await _container.DeleteAsync(item, cancellationToken);
                }
            }
            finally
            {
                if (!cancellationToken.IsCancellationRequested && Directory.Exists(_container.RootDirectory))
                {
                    Directory.Delete(_container.RootDirectory, true);
                }
            }
        }
    }
}
