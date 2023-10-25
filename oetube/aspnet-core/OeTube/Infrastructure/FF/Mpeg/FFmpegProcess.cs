using System.Diagnostics;
using OeTube.Infrastructure.ProcessTemplate;
using Volo.Abp.DependencyInjection;


namespace OeTube.Infrastructure.FF.Mpeg
{

    public class FFmpegProcess : FFProcess<ProcessResult>, ITransientDependency
    {
        private static readonly string FFmpegExe = Path.Combine(FFmpegDir, "ffmpeg.exe");

        public override string PostArguments => "-y";
        public override string ExePath => FFmpegExe;

        protected override ProcessResult HandleProcessOutput(Process process, ProcessSettings arguments, string standardOutput, string standardError)
        {
            return new ProcessResult(arguments, process.StartTime, process.ExitTime);
        }
    }
}
