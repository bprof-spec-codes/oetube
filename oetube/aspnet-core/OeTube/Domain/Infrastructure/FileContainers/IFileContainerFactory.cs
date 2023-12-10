namespace OeTube.Domain.Infrastructure.FileContainers
{
    public interface IFileContainerFactory
    {
        IFileContainer Create<TRelatedType>();
    }
}