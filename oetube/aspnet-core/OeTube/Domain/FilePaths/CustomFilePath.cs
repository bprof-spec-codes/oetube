using Microsoft.IdentityModel.Tokens;

namespace OeTube.Domain.FilePaths
{
    public sealed class CustomFilePath : FilePath
    {
        public CustomFilePath(object key, string subPath)
        {
            var keyStr = key?.ToString();
            if (string.IsNullOrWhiteSpace(keyStr))
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = keyStr;
            var parts = subPath.Split(Path.DirectorySeparatorChar);
            if (parts.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(subPath), subPath);
            }
            else if (parts.Length == 1)
            {
                Name = parts[0];
            }
            else
            {
                this.subPath = parts[0..^1];
                Name = parts[^1];
            }
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentNullException(nameof(subPath), subPath);
            }
            Name = Path.GetFileNameWithoutExtension(Name);
        }

        public override string Name { get; }
        public override string Key { get; }
        private readonly string[] subPath = Array.Empty<string>();
        public override IEnumerable<string> SubPath => subPath;
    }
}