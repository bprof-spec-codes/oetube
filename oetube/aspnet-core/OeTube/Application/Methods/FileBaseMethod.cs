using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods
{
    public abstract class FileBaseMethod<TEntity, TKey> : GetBaseMethod<TEntity, TKey>
         where TEntity : class, IEntity<TKey>
    {
        protected virtual IFileContainer FileContainer { get; }

        protected FileBaseMethod(IAbpLazyServiceProvider serviceProvider,
                                      IReadRepository<TEntity, TKey> repository,
                                      IFileContainer fileContainer) : base(serviceProvider, repository)
        {
            FileContainer = fileContainer;
        }
    }
}