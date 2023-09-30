namespace OeTube.Infrastructure.ProcessTemplate
{
    public class NamedArguments
    {
        public string Arguments { get; }
        public string? Name { get; }

        public NamedArguments(string arguments, string? name = null)
        {
            Arguments = arguments;
            Name = name;
        }

    }
}