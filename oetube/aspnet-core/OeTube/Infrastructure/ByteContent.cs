using Microsoft.AspNetCore.Routing.Constraints;
using Volo.Abp.Content;
using Volo.Abp.Http;

namespace OeTube.Infrastructure
{
    public class ByteContent
    {
        public static async Task<ByteContent> FromStreamAsync(string? name,Stream stream,CancellationToken cancellationToken=default)
        {
            return new ByteContent(name,await stream.GetAllBytesAsync());
        }
        public static async Task<ByteContent> FromRemoteStreamContentAsync(IRemoteStreamContent content,CancellationToken cancellationToken=default)
        {
            return await FromStreamAsync(content.FileName, content.GetStream(), cancellationToken);
        }
        public ByteContent(string? path, byte[] bytes)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            Path = path;
            Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }
        public string Path { get; }
        public string Name => System.IO.Path.GetFileName(Path);
        public string Format => System.IO.Path.GetExtension(Path).Trim('.');
        public byte[] Bytes { get; }
        public string ContentType => MimeTypes.GetByExtension(Format);
        public ByteContent WithNewPath(params string[] path)
        {
            return new ByteContent(System.IO.Path.Combine(path), Bytes);
        }
        public ByteContent WithNewFormat(string format)
        {
            return WithNewPath(System.IO.Path.ChangeExtension(Path, format));
        }
        public Stream GetStream()
        {
            return new MemoryStream(Bytes,true);
        }
        public IRemoteStreamContent GetRemoteStreamContent()
        {
            return new RemoteStreamContent(GetStream(), Path, ContentType);
        }

    }
}
