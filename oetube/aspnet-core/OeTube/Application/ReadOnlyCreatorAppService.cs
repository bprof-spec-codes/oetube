using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application
{
    public abstract class ReadOnlyCreatorAppService
      <TRepository, TEntity, TKey, TOutputDto, TOutputListItemDto, TQueryArgs, TQueryArgsDto>
       : ReadOnlyCustomAppService<TRepository, TEntity, TKey, TOutputDto, TOutputListItemDto, TQueryArgs, TQueryArgsDto>,
       IReadOnlyAppService<TOutputDto, TOutputListItemDto, TKey, TQueryArgsDto>
       where TRepository :
        IReadRepository<TEntity, TKey>, IQueryRepository<TEntity, TQueryArgs>, IUpdateRepository<TEntity, TKey>
       where TEntity : class, IEntity<TKey>, IMayHaveCreator
       where TQueryArgs : IQueryArgs
       where TQueryArgsDto : TQueryArgs
       where TOutputDto : IMayHaveCreatorDto
       where TOutputListItemDto : IMayHaveCreatorDto
    {
        protected IUserRepository UserRepository { get; }
        protected ReadOnlyCreatorAppService(TRepository repository, IFileContainerFactory fileContainerFactory,IUserRepository userRepository) : base(repository, fileContainerFactory)
        {
            UserRepository = userRepository;
            CheckCreatorEnabled = true;
        }

        public override Task<TOutputDto> GetAsync(TKey id)
        {
            return GetAsync<TEntity,TKey,TOutputDto>(Repository, id, UserRepository);
        }
        public override Task<PagedResultDto<TOutputListItemDto>> GetListAsync(TQueryArgsDto input)
        {
            return GetListAsync<TEntity, TOutputListItemDto, TQueryArgs, TQueryArgsDto>(Repository, input, UserRepository);
        }
    }
}