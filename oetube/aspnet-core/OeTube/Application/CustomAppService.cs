﻿using OeTube.Domain.Infrastructure.FileClasses;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Domain.Managers;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

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
        protected virtual async Task UpdateAsync<TEntity>(Func<Task> updateMethod,TEntity entity)
        {
            await CheckPolicyAsync(UpdatePolicy);
            CheckCreator(entity);
            await updateMethod();
        }
        protected virtual async Task<TOutputDto> UpdateAsync
            <TEntity, TKey, TOutputDto, TInputDto>(IUpdateRepository<TEntity,TKey> repository, TKey id, TInputDto input)
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

        protected virtual async Task<TOutputDto> CreateAsync<TEntity, TKey, TOutputDto, TInputDto>
            (ICreateRepository<TEntity,TKey> repository, TInputDto input)
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

        protected virtual async Task DeleteAsync<TEntity, TKey>(IDeleteRepository<TEntity,TKey> repository, TKey id)
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

        protected virtual async Task<TOutputDto> GetAsync<TEntity, TKey, TOutputDto>
            (IReadRepository<TEntity,TKey> repository, TKey id)
        where TEntity : class, IEntity<TKey>
        {
            async Task<TEntity> Get()
            {
                return await repository.GetAsync(id);
            }
            return await GetAsync<TEntity, TOutputDto>(Get);
        }
        protected virtual async Task<IRemoteStreamContent> GetFileAsync(Func<Task<ByteContent>> getFileMethod)
        {
            await CheckPolicyAsync(GetPolicy);
            var file = await getFileMethod();
            return file.GetRemoteStreamContent();
        }
        protected virtual async Task<IRemoteStreamContent?> GetFileOrNullAsync(Func<Task<ByteContent?>> getFileMethod)
        {
            await CheckPolicyAsync(GetPolicy);
            var file = await getFileMethod();
            if(file is null) return null;
            return file.GetRemoteStreamContent();
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
            <TEntity, TOutputListItemDto, TQueryArgs, TQueryArgsDto>
            (IQueryRepository<TEntity,TQueryArgs> repository, TQueryArgsDto queryArgsDto)
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

    public abstract class ReadOnlyCustomAppService
        <TManager,TRepository, TEntity, TKey,TFileClass, 
        TOutputDto, TOutputListItemDto, TQueryArgs, TQueryArgsDto>
       : CustomAppService, IReadOnlyAppService<TOutputDto, TOutputListItemDto, TKey, TQueryArgsDto>
       where TManager : 
        DomainManager<TRepository,TEntity,TKey,TQueryArgs,TFileClass>
       where TRepository :
        IReadRepository<TEntity, TKey>, IQueryRepository<TEntity, TQueryArgs>, IUpdateRepository<TEntity, TKey>
       where TEntity : class, IEntity<TKey>
       where TQueryArgs : IQueryArgs
       where TFileClass : IFileClass
       where TQueryArgsDto : TQueryArgs
    {
        protected TManager Manager { get; }

        protected ReadOnlyCustomAppService(TManager manager)
        {
            Manager = manager;
        }
        public async Task<TOutputDto> GetAsync(TKey id)
        {
            return await GetAsync<TEntity, TKey, TOutputDto>(Manager, id);
        }

        public async Task<PagedResultDto<TOutputListItemDto>> GetListAsync(TQueryArgsDto input)
        {
            return await GetListAsync<TEntity, TOutputListItemDto, TQueryArgs, TQueryArgsDto>(Manager, input);
        }
    }
}