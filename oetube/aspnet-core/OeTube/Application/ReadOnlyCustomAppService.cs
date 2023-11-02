using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application
{
    public abstract class ReadOnlyCustomAppService
        <TRepository, TEntity, TKey, TOutputDto, TOutputListItemDto, TQueryArgs, TQueryArgsDto>
       : CustomAppService, IReadOnlyAppService<TOutputDto, TOutputListItemDto, TKey, TQueryArgsDto>
       where TRepository :
        IReadRepository<TEntity, TKey>, IQueryRepository<TEntity, TQueryArgs>, IUpdateRepository<TEntity, TKey>
       where TEntity : class, IEntity<TKey>
       where TQueryArgs : IQueryArgs
       where TQueryArgsDto : TQueryArgs
    {
        protected IFileContainer FileContainer { get; }
        protected TRepository Repository { get; }
        protected ReadOnlyCustomAppService(TRepository repository,IFileContainerFactory fileContainerFactory)
        {
           Repository=repository;
            FileContainer = fileContainerFactory.Create<TEntity>();
        }
        public virtual async Task<TOutputDto> GetAsync(TKey id)
        {
            return await GetAsync<TEntity, TKey, TOutputDto>(Repository, id);
        }

        public virtual async Task<PagedResultDto<TOutputListItemDto>> GetListAsync(TQueryArgsDto input)
        {
            return await GetListAsync<TEntity, TOutputListItemDto, TQueryArgs, TQueryArgsDto>(Repository, input);
        }
    }
}