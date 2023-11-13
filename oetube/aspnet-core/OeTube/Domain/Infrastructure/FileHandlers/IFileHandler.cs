using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.Videos;

namespace OeTube.Domain.Infrastructure.FileHandlers
{
    public interface IFileHandler
    {
        Task HandleFileAsync<TRelatedType>(CancellationToken cancellationToken = default);
    }
    public interface IFileHandler<TArguments>
    {
        Task HandleFileAsync<TRelatedType>(TArguments args, CancellationToken cancellationToken = default);
    }
    public interface IFileHandler<TArguments, TOutput>
    {
        Task<TOutput> HandleFileAsync<TRelatedType>(TArguments args, CancellationToken cancellationToken = default);
    }

}