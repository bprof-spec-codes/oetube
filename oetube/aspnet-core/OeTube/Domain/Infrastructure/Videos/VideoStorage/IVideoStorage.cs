namespace OeTube.Domain.Infrastructure.VideoStorage
{
    public interface IVideoStorage : IVideoStoragePath, IVideoStorageRead, IVideoStorageSave, IVideoStorageDelete
    {
    }
}