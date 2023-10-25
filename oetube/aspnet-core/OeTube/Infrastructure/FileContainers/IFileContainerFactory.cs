namespace OeTube.Infrastructure.FileContainers
{
    public interface IFileContainerFactory
    {
        IFileContainer Create(string containerName);
    }
}