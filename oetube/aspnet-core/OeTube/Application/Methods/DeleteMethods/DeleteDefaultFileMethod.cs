using OeTube.Application.AuthorizationCheckers;
using OeTube.Domain.FilePaths;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods.DeleteMethods
{
    public class DeleteDefaultFileMethod<TEntity, TKey, TInputFilePath> :DeleteFileMethod<TEntity,TKey,TInputFilePath>
        where TEntity : class, IEntity<TKey>
        where TInputFilePath : IDefaultFilePath
    {
        public DeleteDefaultFileMethod(IAbpLazyServiceProvider serviceProvider,
                                       IReadRepository<TEntity, TKey> repository,
                                       IFileContainer fileContainer) : base(serviceProvider, repository, fileContainer)
        {
        }
        public virtual async Task DeleteDefaultFileAsync()
        {
            await CheckPolicyAsync();
            await FileContainer.DeleteDefaultFileAsync<TInputFilePath>();
        }
    }
}
