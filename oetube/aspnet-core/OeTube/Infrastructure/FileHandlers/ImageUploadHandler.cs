using OeTube.Domain.Configs;
using OeTube.Domain.FilePaths;
using OeTube.Domain.FilePaths.ImageFiles;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Imaging;

namespace OeTube.Infrastructure.FileHandlers
{
    public class ImageUploadHandler : ImageHandler, IImageUploadHandler, ITransientDependency
    {
        public ImageUploadHandler(IFileContainerFactory fileContainerFactory, IImageResizer resizer, ISourceImageFileConfig sourceImageConfig, IThumbnailImageFileConfig thumbnailConfig) : base(fileContainerFactory, resizer, sourceImageConfig, thumbnailConfig)
        {
        }

        public async Task HandleFileAsync<TRelatedType>(ImageUploadHandlerArgs args, CancellationToken cancellationToken = default)
        {
            var container = _fileContainerFactory.Create<TRelatedType>();

            await SaveImageAsync(container, new SourceImagePath(args.Id), _sourceImageConfig, args.Content, cancellationToken);
            await SaveImageAsync(container, new ThumbnailImagePath(args.Id), _thumbnailConfig, args.Content, cancellationToken);
        }

        private async Task SaveImageAsync(IFileContainer container,
                                          IFilePath path,
                                          IImageFileConfig config,
                                          ByteContent content,
                                          CancellationToken cancellationToken = default)
        {
            content = await ResizeImageASync(config, content, cancellationToken);
            await container.SaveFileAsync(path, content, cancellationToken);
        }
    }
}