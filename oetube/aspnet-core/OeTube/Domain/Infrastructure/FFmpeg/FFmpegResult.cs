﻿using OeTube.Infrastructure.ProcessTemplate;

namespace OeTube.Domain.Infrastructure.FFmpeg
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
}