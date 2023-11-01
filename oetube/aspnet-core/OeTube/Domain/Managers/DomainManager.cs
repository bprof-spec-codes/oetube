using OeTube.Domain.Infrastructure.FileClasses;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Infrastructure.FileClasses;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;

namespace OeTube.Domain.Managers
{
    public abstract class DomainManager<TRepository,TEntity,TKey,TQueryArgs,TFileClass>
        :DomainService, IQueryRepository<TEntity,TQueryArgs>,IReadRepository<TEntity,TKey>,IUpdateRepository<TEntity,TKey>, IFileProvider<TFileClass>
     where TEntity:class,IEntity<TKey>
     where TRepository:IReadRepository<TEntity,TKey>,IQueryRepository<TEntity,TQueryArgs>,IUpdateRepository<TEntity,TKey>
     where TQueryArgs:IQueryArgs
     where TFileClass:IFileClass
    {
        protected IBackgroundJobManager BackgroundJobManager { get; }
        protected IFileContainer FileContainer { get; }
        protected TRepository Repository { get; }
        protected ILocalEventBus LocalEventBus { get; }

        public DomainManager(TRepository repository, IFileContainerFactory containerFactory,IBackgroundJobManager backgroundJobManager,ILocalEventBus localEventBus)
        {
            Repository = repository;
            FileContainer = containerFactory.Create<TEntity>();
            BackgroundJobManager = backgroundJobManager;
            LocalEventBus = localEventBus;
        }

        public virtual async Task<ByteContent?> GetFileOrNullAsync(TFileClass file,CancellationToken cancellationToken=default)
        {
            return await FileContainer.GetFileOrNullAsync(file, cancellationToken);
        }

        public virtual async Task<ByteContent> GetFileAsync(TFileClass file,CancellationToken cancellationToken=default)
        {
            return await FileContainer.GetFileAsync(file, cancellationToken);
        }

        public virtual Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return Repository.GetAsync(id, includeDetails, cancellationToken);
        }
        public virtual Task<List<TEntity>> GetListAsync(TQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetListAsync(args, includeDetails, cancellationToken);
        }
        public virtual Task<List<TEntity>> GetManyAsync(IEnumerable<TKey> ids, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Repository.GetManyAsync(ids, includeDetails, cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return await Repository.UpdateAsync(entity, autoSave, cancellationToken);
        }

        public async Task UpdateManyAsync(IEnumerable<TEntity> entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await Repository.UpdateManyAsync(entity, autoSave, cancellationToken);
        }
    }


}
