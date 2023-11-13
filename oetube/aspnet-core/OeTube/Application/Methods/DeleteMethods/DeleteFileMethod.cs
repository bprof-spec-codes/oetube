using OeTube.Domain.FilePaths;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods.DeleteMethods
{
    public class DeleteFileMethod<TEntity, TKey, TInputFilePath> : FileBaseMethod<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
            where TInputFilePath : IFilePath
    {
        public DeleteFileMethod(IAbpLazyServiceProvider serviceProvider,
                                IReadRepository<TEntity, TKey> repository,
                                IFileContainer fileContainer) : base(serviceProvider, repository, fileContainer)
        {
        }

        public virtual async Task DeleteAsync(TKey id, TInputFilePath input)
        {
            await CheckPolicyAsync();
            var entity = await Repository.GetAsync(id);
            await CheckRightsAsync(entity);
            await FileContainer.DeleteFileAsync(input);
        }
    }
}