using Microsoft.AspNetCore.Authorization;
using OeTube.Application.Dtos;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Domain.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;
using OeTube.Application.AuthorizationCheckers;
using System.Diagnostics.CodeAnalysis;

namespace OeTube.Application.Methods.GetListMethods
{

    public class GetListMethod<TEntity, TQueryArgs, TOutputListItemDto> : ApplicationMethod
            where TEntity : class, IEntity
            where TQueryArgs : IQueryArgs
    {
        public GetListMethod(IAbpLazyServiceProvider serviceProvider, IQueryRepository<TEntity, TQueryArgs> repository) : base(serviceProvider)
        {
            Repository = repository;
        }

        protected virtual IQueryRepository<TEntity, TQueryArgs> Repository { get; }
        protected virtual async Task<PaginationResult<TEntity>> GetListByQueryAsync(TQueryArgs queryArgs)
        {
            if(Repository is IQueryAvaliableRepository<TEntity,TQueryArgs> queryAvaliable)
            {
                return await queryAvaliable.GetAvaliableAsync(Authorization?.CurrentUser.Id, queryArgs);
            }
            else
            {
                return await Repository.GetListAsync(queryArgs);
            }
        }

        public virtual async Task<PaginationDto<TOutputListItemDto>> GetListAsync(TQueryArgs input)
        {
            await CheckPolicyAsync();
            PaginationResult<TEntity> pagination = await GetListByQueryAsync(input);
            if(Authorization is IAuthorizationManyChecker<TEntity> manyChecker)
            {
                await manyChecker.CheckRightsManyAsync(pagination);
            }
            var dtos = new PaginationDto<TOutputListItemDto>()
            {
                Items = new List<TOutputListItemDto>(),
                CurrentPage=pagination.CurrentPage,
                PageCount=pagination.PageCount,
                TotalCount=pagination.TotalCount
            };
            foreach (var entity in pagination)
            {
                var dto = await MapAsync<TEntity, TOutputListItemDto>(entity);
                dtos.Items.Add(dto);
            }
            return dtos;
        }
    }
}
