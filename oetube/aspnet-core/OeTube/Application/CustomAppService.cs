using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application
{
    public abstract class CustomAppService : ApplicationService
    {
        protected bool CheckCreatorEnabled { get; set; }
        protected virtual string? GetPolicy { get; set; }
        protected virtual string? GetListPolicy { get; set; }
        protected virtual string? DeletePolicy { get; set; }
        protected virtual string? UpdatePolicy { get; set; }
        protected virtual string? CreatePolicy { get; set; }

        protected virtual async Task<TOutputDto> UpdateAsync<TEntity, TOutputDto>(Func<Task<TEntity>> updateMethod, TEntity entity)
        {
            await CheckPolicyAsync(UpdatePolicy);
            CheckCreator(entity);
            var updateEntity = await updateMethod();
            return await Task.FromResult(ObjectMapper.Map<TEntity, TOutputDto>(updateEntity));
        }

        protected virtual async Task<TOutputDto> UpdateAsync
            <TRepository, TEntity, TKey, TOutputDto, TInputDto>(TRepository repository, TKey id, TInputDto input)
            where TRepository : IUpdateRepository<TEntity, TKey>
            where TEntity : class, IEntity<TKey>
        {
            var entity = ObjectMapper.Map(input, await repository.GetAsync(id));
            return await UpdateAsync<TEntity, TOutputDto>(Update, entity);
            async Task<TEntity> Update()
            {
                return await repository.UpdateAsync(entity);
            }
        }

        protected virtual void CheckCreator<TEntity>(TEntity entity)
        {
            if (!CheckCreatorEnabled)
            {
                return;
            }

            if ((entity is IMayHaveCreator mayHave && mayHave.CreatorId is not null && mayHave.CreatorId != CurrentUser.Id)
                || entity is IMustHaveCreator mustHave && mustHave.CreatorId != CurrentUser.Id)
            {
                throw new InvalidOperationException();
            }
        }

        protected virtual async Task<TOutputDto> CreateAsync<TEntity, TOutputDto>(Func<Task<TEntity>> createMethod)
            where TEntity : IEntity

        {
            await CheckPolicyAsync(CreatePolicy);
            var entity = await createMethod();
            return await Task.FromResult(ObjectMapper.Map<TEntity, TOutputDto>(entity));
        }

        protected virtual async Task<TOutputDto> CreateAsync<TRepository, TEntity, TKey, TOutputDto, TInputDto>
            (TRepository repository, TInputDto input)
            where TRepository : ICreateRepository<TEntity, TKey>
            where TEntity : class, IEntity<TKey>
        {
            async Task<TEntity> Create()
            {
                var entity = await Task.FromResult(ObjectMapper.Map<TInputDto, TEntity>(input));
                await repository.InsertAsync(entity, true);
                return entity;
            }
            return await CreateAsync<TEntity, TOutputDto>(Create);
        }

        protected virtual async Task DeleteAsync<TRepository, TEntity, TKey>(TRepository repository, TKey id)
            where TRepository : IDeleteRepository<TEntity, TKey>
            where TEntity : class, IEntity<TKey>
        {
            await CheckPolicyAsync(DeletePolicy);
            var entity = await repository.GetAsync(id);
            CheckCreator(entity);
            await repository.DeleteAsync(entity, true);
        }

        protected virtual async Task<TOutputDto> GetAsync<TEntity, TOutputDto>(Func<Task<TEntity>> getMethod)
        {
            await CheckPolicyAsync(GetPolicy);
            var entity = await getMethod();
            var output = await Task.FromResult(ObjectMapper.Map<TEntity, TOutputDto>(entity));
            return output;
        }

        protected virtual async Task<TOutputDto> GetAsync<TRepository, TEntity, TKey, TOutputDto>(TRepository repository, TKey id)
        where TRepository : IReadRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        {
            async Task<TEntity> Get()
            {
                return await repository.GetAsync(id);
            }
            return await GetAsync<TEntity, TOutputDto>(Get);
        }

        protected virtual async Task<PagedResultDto<TOutputListItemDto>> GetListAsync<TEntity, TOutputListItemDto>
            (Func<Task<List<TEntity>>> getListMethod)
        {
            await CheckPolicyAsync(GetListPolicy);
            var list = await getListMethod();
            var dtos = new List<TOutputListItemDto>();
            foreach (var entity in list)
            {
                var dto = await Task.FromResult(ObjectMapper.Map<TEntity, TOutputListItemDto>(entity));
                dtos.Add(dto);
            }
            return new PagedResultDto<TOutputListItemDto>()
            {
                Items = dtos,
                TotalCount = dtos.Count
            };
        }

        protected virtual async Task<PagedResultDto<TOutputListItemDto>> GetListAsync
            <TRepository, TEntity, TOutputListItemDto, TQueryArgs, TQueryArgsDto>(TRepository repository, TQueryArgsDto queryArgsDto)
            where TRepository : IQueryRepository<TEntity, TQueryArgs>
            where TQueryArgs : IQueryArgs
            where TQueryArgsDto : TQueryArgs
            where TEntity : class, IEntity
        {
            async Task<List<TEntity>> GetList()
            {
                return await repository.GetListAsync(queryArgsDto);
            }
            return await GetListAsync<TEntity, TOutputListItemDto>(GetList);
        }
    }

    public abstract class ReadOnlyCustomAppService<TRepository, TEntity, TKey, TOutputDto, TOutputListItemDto, TQueryArgs, TQueryArgsDto>
       : CustomAppService, IReadOnlyAppService<TOutputDto, TOutputListItemDto, TKey, TQueryArgsDto>
       where TRepository : IReadRepository<TEntity, TKey>, IQueryRepository<TEntity, TQueryArgs>
       where TEntity : class, IEntity<TKey>
       where TQueryArgs : IQueryArgs
       where TQueryArgsDto : TQueryArgs
    {
        protected TRepository Repository { get; }

        protected ReadOnlyCustomAppService(TRepository repository)
        {
            Repository = repository;
        }

        public async Task<TOutputDto> GetAsync(TKey id)
        {
            return await GetAsync<TRepository, TEntity, TKey, TOutputDto>(Repository, id);
        }

        public async Task<PagedResultDto<TOutputListItemDto>> GetListAsync(TQueryArgsDto input)
        {
            return await GetListAsync<TRepository, TEntity, TOutputListItemDto, TQueryArgs, TQueryArgsDto>(Repository, input);
        }
    }
}