namespace OeTube.Infrastructure.ProcessTemplate
{
    public class ProcessSettings
    {
        public NamedArguments NamedArguments { get; private set; }
        public string WorkingDirectory { get; private set; }
        public bool WriteToDebug { get; private set; }
        public ProcessSettings(NamedArguments namedArguments, string workingDirectory = "", bool writeToDebug = false)
        {
            NamedArguments = namedArguments;
            WriteToDebug = writeToDebug;
            WorkingDirectory = workingDirectory;
        }
        public ProcessSettings WithNewArguments(string arguments, string? name = null)
        {
            return new ProcessSettings(new NamedArguments(arguments, name), WorkingDirectory, WriteToDebug);
        }
    }
}
