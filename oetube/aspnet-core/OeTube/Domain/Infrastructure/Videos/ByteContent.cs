using Volo.Abp.Content;
using Volo.Abp.Http;

namespace OeTube.Domain.Infrastructure.Videos
{
    public class ByteContent
    {
        public static async Task<ByteContent> FromStreamAsync(string? name, Stream stream, CancellationToken cancellationToken = default)
        {
            return new ByteContent(name, await stream.GetAllBytesAsync(cancellationToken));
        }

        public static async Task<ByteContent> FromRemoteStreamContentAsync(IRemoteStreamContent? content, CancellationToken cancellationToken = default)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return await FromStreamAsync(content.FileName, content.GetStream(), cancellationToken);
        }

        public ByteContent(string? path, byte[] bytes, string? contentType = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            Path = path;
            Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
            ContentType = contentType ?? MimeTypes.GetByExtension(Format);
        }

        public string Path { get; }
        public string Format => System.IO.Path.GetExtension(Path).Trim('.');
        public byte[] Bytes { get; }
        public string ContentType { get; }
        public string FileName => System.IO.Path.GetFileName(Path);
        public string FileNameWithoutFormat => System.IO.Path.GetFileNameWithoutExtension(Path);

        public ByteContent WithNewName(string name)
        {
            string path = System.IO.Path.Combine(Path.Replace(FileName, ""), $"{name}.{Format}");
            return new ByteContent(path, Bytes, ContentType);
        }

        public ByteContent WithNewPath(string path)
        {
            return new ByteContent(path, Bytes, ContentType);
        }

        public ByteContent WithNewFormat(string format)
        {
            return WithNewPath(System.IO.Path.ChangeExtension(Path, format));
        }

        public ByteContent WithNewContentType(string contentType)
        {
            return new ByteContent(Path, Bytes, contentType);
        }

        public Stream GetStream()
        {
            return new MemoryStream(Bytes, true);
        }

        public IRemoteStreamContent GetRemoteStreamContent(string? contentType = null)
        {
            return new RemoteStreamContent(GetStream(), Path, contentType ?? ContentType);
        }
    }
}