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

    public interface IContentFileHandler
    {
        Task HandleFileAsync<TRelatedType>(ByteContent content, CancellationToken cancellationToken = default);
    }
    public interface IContentFileHandler<TArguments>
    {
        Task HandleFileAsync<TRelatedType>(ByteContent content, TArguments args, CancellationToken cancellationToken = default);
    }
    public interface IContentFileHandler<TArguments, TOutput>
    {
        Task<TOutput> HandleFileAsync<TRelatedType>(ByteContent content, TArguments args, CancellationToken cancellationToken = default);
    }

}