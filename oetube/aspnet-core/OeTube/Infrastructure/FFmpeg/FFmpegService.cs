using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OeTube.Infrastructure.FFmpeg.Job;
using OeTube.Infrastructure.FFprobe;
using OeTube.Infrastructure.FFprobe.Infos;
using OeTube.Infrastructure.FileContainers;
using OeTube.Infrastructure.ProcessTemplate;
using OeTube.Infrastructure.VideoStorage;
using System.Runtime.CompilerServices;
using System.Threading;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFmpeg
{
    public class FFmpegResult
    {
        public ProcessResult Result { get; }
        public IReadOnlyCollection<string> OutputFiles { get; }

        public FFmpegResult(ProcessResult result, IReadOnlyCollection<string> outputFiles)
        {
            Result = result;
            OutputFiles = outputFiles;
        }

    }
    public class FFmpegService : IFFmpegService
    {
        private readonly IFileContainer _container;
        private readonly FFmpegProcess _ffmpeg;
        public Guid Id { get; }
        public bool WriteToDebug { get; set; }
        private string LocalRootDirectory => _container.GetAbsolutePath(Id.ToString());
        public FFmpegService(FFmpegProcess ffmpeg, IFileContainerFactory containerFactory)
        {
            _ffmpeg = ffmpeg;
            _container = containerFactory.Create("ffmpeg");
            Id = Guid.NewGuid();
        }
        private string GetLocalPathFromAbsolutePath(string path)
        {
            return path.Replace(LocalRootDirectory + Path.DirectorySeparatorChar, "");
        }
        private string GetContainerPathFromLocalPath(string path)
        {
            return Path.Combine(Id.ToString(), path);
        }
        public async Task<bool> DeleteAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _container.DeleteAsync(name, cancellationToken);
        }
        public async Task<ByteContent> GetContentAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _container.GetContentAsync(GetContainerPathFromLocalPath(name), cancellationToken);
        }

        public HashSet<string> GetFiles()
        {
            string directory = _container.GetAbsolutePath(Id.ToString());
            var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            return files.Select(GetLocalPathFromAbsolutePath).ToHashSet();
        }
        public async Task<FFmpegResult> ExecuteAsync(ByteContent? input, string arguments, string? processName = null, CancellationToken cancellationToken = default)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            arguments = $"-i {input.Path} {arguments}";

            input = input.WithNewPath(GetContainerPathFromLocalPath(input.Path));

            await _container.SaveAsync(input, true, cancellationToken);

            var settings = new ProcessSettings(
                new NamedArguments(arguments, processName ?? input.Path),
                LocalRootDirectory,
                WriteToDebug);

            var files = GetFiles();
            var result = await _ffmpeg.StartProcessAsync(settings, cancellationToken);
            files.ExceptWith(GetFiles());
            return new FFmpegResult(result, files);
        }
        public async Task CleanUpAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var files = GetFiles().Select(GetContainerPathFromLocalPath);
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
                if (!cancellationToken.IsCancellationRequested && Directory.Exists(LocalRootDirectory))
                {
                    Directory.Delete(LocalRootDirectory, true);
                }
            }
        }
    }
}
