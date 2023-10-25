using OeTube.Infrastructure.FileContainers;

namespace OeTube.Infrastructure.VideoStorages
{
    public interface IVideoStorageMethodFactory
    {
        IVideoStorageDelete CreateDelete(IFileContainer container, IVideoStoragePath path);
        IVideoStorageGet CreateGet(IFileContainer container, IVideoStoragePath path);
        IVideoStoragePath CreatePath();
        IVideoStorageSave CreateSave(IFileContainer container, IVideoStoragePath path);
    }
}