using OeTube.Domain.Infrastructure.Videos;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
using Volo.Abp.Http;
using Microsoft.SqlServer.Server;

namespace OeTube.Domain.FilePaths
{

    public abstract class FilePath : IFilePath
    {
        public virtual string Key => string.Empty;
        public virtual IEnumerable<string> SubPath => Enumerable.Empty<string>();
        public abstract string Name { get; }

        public virtual string GetAbsoluteKeyDirectory(string absoluteRootDirectory)
        {
            return Path.Combine(absoluteRootDirectory, GetKeyDirectory());
        }
        public virtual string GetAbsoluteSubPathDirectory(string absoluteRootDirectory)
        {
            return Path.Combine(absoluteRootDirectory, GetSubpathDirectory());
        }
        public virtual string GetAbsolutePath(string absoluteRootDirectory, string? format = null)
        {

            return Path.Combine(absoluteRootDirectory, GetPath(format));
        }
        public virtual string GetKeyDirectory()
        {
            return Key;
        }
        public virtual string GetSubpathDirectory()
        {
            return Path.Combine(Key, Path.Combine(SubPath.ToArray()));
        }
        public virtual string GetPath(string? format = null)
        {
            format ??= string.Empty;
            format = format.TrimStart('.');
            return Path.Combine(GetSubpathDirectory(), Name + "." + format);
        }

        public virtual string CombineWithKey(string localKeyPath)
        {
            return Path.Combine(Key, localKeyPath);
        }
    }
}
