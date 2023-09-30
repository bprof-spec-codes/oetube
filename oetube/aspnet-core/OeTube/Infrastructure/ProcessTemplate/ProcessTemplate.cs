using System.Diagnostics;
using System.Text;

namespace OeTube.Infrastructure.ProcessTemplate
{
    public abstract class ProcessTemplate<TOutput>
    {
        public abstract string ExePath { get; }
        public string FileName => Path.GetFileNameWithoutExtension(ExePath);
        public virtual string PreArguments => "";
        public virtual string PostArguments => "";

        protected abstract TOutput HandleProcessOutput(Process process, ProcessSettings settings, string standardOutput, string standardError);
        protected virtual void HandleStandardOutputData(ProcessSettings settings, DataReceivedEventArgs dataArgs)
        {
            Debug.WriteLineIf(settings.WriteToDebug, "[SOUT] " + dataArgs.Data, settings.NamedArguments.Name);
        }
        protected virtual void HandleStandardErrorData(ProcessSettings settings, DataReceivedEventArgs dataArgs)
        {
            Debug.WriteLineIf(settings.WriteToDebug, "[SERR] " + dataArgs.Data, settings.NamedArguments.Name);
        }
        protected virtual ProcessStartInfo CreateStartInfo(ProcessSettings settings)
        {
            return new ProcessStartInfo()
            {
                FileName = ExePath,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = settings.NamedArguments.Arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = settings.WorkingDirectory
            };
        }
        protected string JoinArguments(params string?[] arguments)
        {
            return string.Join(" ", arguments.Where(a => a is not null));
        }

        public virtual async Task<TOutput> StartProcessAsync(ProcessSettings settings, CancellationToken cancellationToken = default)
        {
            StringBuilder outSb = new StringBuilder();
            StringBuilder errorSb = new StringBuilder();
            settings = settings.WithNewArguments(JoinArguments(PreArguments, settings.NamedArguments.Arguments, PostArguments),
                                               settings.NamedArguments.Name ?? FileName);

            Process process = new Process()
            {
                StartInfo = CreateStartInfo(settings)
            };

            void Output(object sender, DataReceivedEventArgs dataArgs)
            {
                HandleStandardOutputData(settings, dataArgs);
                outSb.AppendLine(dataArgs.Data);
            }
            void Error(object sender, DataReceivedEventArgs dataArgs)
            {
                HandleStandardErrorData(settings, dataArgs);
                errorSb.AppendLine(dataArgs.Data);
            }
            process.OutputDataReceived += Output;
            process.ErrorDataReceived += Error;
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await process.WaitForExitAsync(cancellationToken);
                return HandleProcessOutput(process, settings, outSb.ToString(), errorSb.ToString());
            }
            finally
            {
                process.OutputDataReceived -= Output;
                process.ErrorDataReceived -= Error;
            }
        }

    }
}
