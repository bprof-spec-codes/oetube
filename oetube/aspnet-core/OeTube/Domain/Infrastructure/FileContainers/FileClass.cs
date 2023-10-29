using OeTube.Domain.Infrastructure.Videos;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
using Volo.Abp.Http;
using Microsoft.SqlServer.Server;

namespace OeTube.Domain.Infrastructure.FileContainers
{
    public abstract class FileClass
    {
        public virtual string Key => string.Empty;
        public virtual IEnumerable<string> SubPath => Enumerable.Empty<string>();
        public abstract string Name { get; }


        public virtual string MimeTypeCategory => string.Empty;
        public virtual IEnumerable<string> ExplicitFormats => Enumerable.Empty<string>();
        public virtual long MaxFileSize => long.MaxValue;

        public virtual string GetAbsoluteKeyDirectory(string absoluteRootDirectory)
        {
            return Path.Combine(absoluteRootDirectory, GetKeyDirectory());
        }
        public virtual string GetAbsoluteSubPathDirectory(string absoluteRootDirectory)
        {
            return Path.Combine(absoluteRootDirectory, GetSubpathDirectory());
        }
        public virtual string GetAbsolutePath(string absoluteRootDirectory,string? format=null)
        {
        
            return Path.Combine(absoluteRootDirectory,GetPath(format));
        }
        public virtual string GetKeyDirectory()
        {
            return Key;
        }
        public virtual string GetSubpathDirectory()
        {
            return Path.Combine(Key,Path.Combine(SubPath.ToArray()));
        }
        public virtual string GetPath(string? format=null)
        {
            format ??= string.Empty;
            format = format.TrimStart('.');
            return Path.Combine(GetSubpathDirectory(),Name+"."+format);
        }

        public virtual string CombineWithKey(string localKeyPath)
        {
            return Path.Combine(Key, localKeyPath);
        }

        public virtual void CheckContent(ByteContent content)
        {
            if (!string.IsNullOrEmpty(MimeTypeCategory))
            {
                var typeParts = content.ContentType.Split('/');
                if (typeParts.Length != 2 || typeParts[0].ToLower() != MimeTypeCategory)
                {
                    throw new ArgumentException(content.ContentType, nameof(content));
                }
            }

            if (!ExplicitFormats.IsNullOrEmpty() && !ExplicitFormats.Select(f => f.TrimStart('.').ToLower())
                                                                 .Contains(content.Format.ToLower()))
            {
                throw new ArgumentException(content.Format, nameof(content));
            }

            if (content.Bytes.LongLength > MaxFileSize)
            {
                throw new ArgumentException(content.Bytes.Length.ToString(), nameof(content));
            }
        }
    }
}
