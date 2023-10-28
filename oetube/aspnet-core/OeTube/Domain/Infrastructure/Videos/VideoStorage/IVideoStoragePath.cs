using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Infrastructure.VideoStorage
{
    public interface IVideoStoragePath
    {
        string SelectedFrameName { get; }
        string HlsListName { get; }
        string HlsListFormat { get; }
        string HlsSegmentFormat { get; }
        string SourceName { get; }
        string ResizedName { get; }

        string GetFramePath(Guid id, int index, string? format = null);

        string GetFramesDirectoryPath(Guid id);

        string GetHlsListPath(Guid id, Resolution resolution);

        string GetHlsSegmentPath(Guid id, Resolution resolution, int segment);

        string GetResizedPath(Guid id, Resolution resolution, string? format = null);

        string GetResolutionDirectoryPath(Guid id, Resolution resolution);

        string GetSelectedFramePath(Guid id, string? format = null);

        string GetSourcePath(Guid id, string? format = null);
    }
}