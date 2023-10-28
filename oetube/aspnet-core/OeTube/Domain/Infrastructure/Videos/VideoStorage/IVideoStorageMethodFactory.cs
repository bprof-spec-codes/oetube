using OeTube.Infrastructure.FileContainers;

namespace OeTube.Domain.Infrastructure.VideoStorage
{
    public interface IVideoStorageMethodFactory
    {
        IVideoStorageDelete CreateDelete(IFileContainer container, IVideoStoragePath path);

        IVideoStorageRead CreateRead(IFileContainer container, IVideoStoragePath path);

        IVideoStoragePath CreatePath();

        IVideoStorageSave CreateSave(IFileContainer container, IVideoStoragePath path);
    }
}