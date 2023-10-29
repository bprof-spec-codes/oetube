using Volo.Abp.Content;
using Volo.Abp.Http;

namespace OeTube.Domain.Infrastructure.Videos
{
    public class ByteContent
    {
        public static async Task<ByteContent> FromStreamAsync(string format, Stream stream, CancellationToken cancellationToken = default)
        {
            return new ByteContent(format, await stream.GetAllBytesAsync(cancellationToken));
        }

        public static async Task<ByteContent> FromRemoteStreamContentAsync(IRemoteStreamContent? content, CancellationToken cancellationToken = default)
        {
            if (content?.FileName is null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            
            return await FromStreamAsync(Path.GetExtension(content.FileName), content.GetStream(), cancellationToken);
        }

        public ByteContent(string format, byte[] bytes, string? contentType = null)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                throw new ArgumentException($"'{nameof(format)}' cannot be null or whitespace.", nameof(format));
            }
            ContentType = contentType ?? MimeTypes.GetByExtension(format);
            Format = format.TrimStart('.');
            Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }

        public string Format { get; }
        public byte[] Bytes { get; }
        public string ContentType { get; }

    
        public ByteContent WithNewFormat(string format)
        {
            return new ByteContent(format, Bytes, ContentType);
        }

        public ByteContent WithNewContentType(string contentType)
        {
            return new ByteContent(Format, Bytes, contentType);
        }

        public Stream GetStream()
        {
            return new MemoryStream(Bytes, true);
        }

        public IRemoteStreamContent GetRemoteStreamContent(string? contentType = null)
        {
            return new RemoteStreamContent(GetStream(),"content."+Format, contentType ?? ContentType);
        }
    }
}