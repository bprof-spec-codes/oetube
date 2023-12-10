using OeTube.Domain.FilePaths.VideoFiles;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FileContainers;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FileHandlers
{
    public class SelectVideoFrameHandler : ISelectVideoFrameHandler, ITransientDependency
    {
        private readonly IFileContainerFactory _fileContainerFactory;

        public SelectVideoFrameHandler(IFileContainerFactory fileContainerFactory)
        {
            _fileContainerFactory = fileContainerFactory;
        }

        public async Task HandleFileAsync<TRelatedType>(SelectVideoFrameHandlerArgs args, CancellationToken cancellationToken = default)
        {
            var container = _fileContainerFactory.Create<TRelatedType>();
            var selectedFrame = await container.GetFileAsync(new FramePath(args.Id, args.Index), cancellationToken);
            await container.SaveFileAsync(new SelectedFramePath(args.Id), selectedFrame, cancellationToken);
        }
    }
}