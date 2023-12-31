﻿using OeTube.Infrastructure.ProcessTemplate;
using System.Diagnostics;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFMpeg
{
    public class FFMpegProcess : FFProcess<ProcessResult>, ITransientDependency
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