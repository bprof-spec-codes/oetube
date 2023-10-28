using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.VideoStorage;

namespace OeTube.Infrastructure.Videos.VideoStorages
{
    public class VideoStoragePath : IVideoStoragePath
    {
        public string SourceName { get; } = "source";
        public string ResizedName { get; } = "resized";
        public string SelectedFrameName { get; } = "selected";
        public string HlsListName { get; } = "list";
        public string HlsListFormat { get; } = "m3u8";
        public string HlsSegmentFormat { get; } = "ts";

        public string GetSourcePath(Guid id, string? format = null)
              => Path.Combine(id.ToString(), $"{SourceName}.{format ?? ""}");

        public string GetResolutionDirectoryPath(Guid id, Resolution resolution)
            => Path.Combine(id.ToString(), resolution.ToString());

        public string GetResizedPath(Guid id, Resolution resolution, string? format = null)
            => Path.Combine(GetResolutionDirectoryPath(id, resolution), $"{ResizedName}.{format ?? ""}");

        public string GetFramesDirectoryPath(Guid id) => Path.Combine(id.ToString(), "frames");

        public string GetFramePath(Guid id, int index, string? format = null)
            => Path.Combine(GetFramesDirectoryPath(id), $"{index}.{format ?? ""}");

        public string GetSelectedFramePath(Guid id, string? format = null)
            => Path.Combine(GetFramesDirectoryPath(id), $"{SelectedFrameName}.{format ?? ""}");

        public string GetHlsListPath(Guid id, Resolution resolution)
            => Path.Combine(GetResolutionDirectoryPath(id, resolution), $"{HlsListName}.{HlsListFormat}");

        public string GetHlsSegmentPath(Guid id, Resolution resolution, int segment)
            => Path.Combine(GetResolutionDirectoryPath(id, resolution), $"{segment}.{HlsSegmentFormat}");
    }
}