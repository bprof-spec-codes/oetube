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
    [ExposeServices(typeof(IDefaultImageUploadHandler))]
    public class DefaulImageUploadHandler : ImageHandler, IDefaultImageUploadHandler, ITransientDependency
    {
        public DefaulImageUploadHandler(IFileContainerFactory fileContainerFactory, IImageResizer resizer, ISourceImageFileConfig sourceImageConfig, IThumbnailImageFileConfig thumbnailConfig) : base(fileContainerFactory, resizer, sourceImageConfig, thumbnailConfig)
        {
        }

        public async Task HandleFileAsync<TRelatedType>(ByteContent content, CancellationToken cancellationToken = default)
        {
            var container = _fileContainerFactory.Create<TRelatedType>();

            await SaveImageAsync<SourceImagePath>(container, _sourceImageConfig, content, cancellationToken);
            await SaveImageAsync<ThumbnailImagePath>(container, _thumbnailConfig, content, cancellationToken);
        }

        private async Task SaveImageAsync<TDefaultFilePath>(IFileContainer container,
                                          IImageFileConfig config,
                                          ByteContent content,
                                          CancellationToken cancellationToken = default)
        where TDefaultFilePath : IDefaultFilePath
        {
            content = await ResizeImageASync(config, content, cancellationToken);
            await container.SaveDefaultFileAsync<TDefaultFilePath>(content, cancellationToken);
        }
    }
}