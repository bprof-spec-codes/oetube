using OeTube.Domain.Configs;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileContainers;
using Volo.Abp.Http;
using Volo.Abp.Imaging;

namespace OeTube.Infrastructure.FileHandlers
{
    public abstract class ImageHandler
    {
        protected readonly IFileContainerFactory _fileContainerFactory;
        protected readonly IImageResizer _resizer;
        protected readonly IThumbnailImageFileConfig _thumbnailConfig;
        protected readonly ISourceImageFileConfig _sourceImageConfig;

        public ImageHandler(IFileContainerFactory fileContainerFactory,
                                  IImageResizer resizer,
                                  ISourceImageFileConfig sourceImageConfig,
                                  IThumbnailImageFileConfig thumbnailConfig)
        {
            _fileContainerFactory = fileContainerFactory;
            _resizer = resizer;
            _sourceImageConfig = sourceImageConfig;
            _thumbnailConfig = thumbnailConfig;
        }

        protected async Task<ByteContent> ResizeImageASync(
                                         IImageFileConfig config,
                                         ByteContent content,
                                         CancellationToken cancellationToken = default)
        {
            var args = new ImageResizeArgs(config.MaxWidth, config.MaxHeight, ImageResizeMode.Max);
            var resized = await _resizer.ResizeAsync(content.Bytes, args, MimeTypes.GetByExtension("." + config.OutputFormat), cancellationToken);
            return new ByteContent(config.OutputFormat, resized.Result);
        }
    }
}