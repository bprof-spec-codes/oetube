using OeTube.Application.AuthorizationCheckers;
using OeTube.Application.Dtos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace OeTube.Application.Methods.GetListMethods
{
    public class GetChildrenListMethod<TEntity, TKey, TChildEntity, TChildQueryArgs, TChildOutputListItemDto>
        : GetBaseMethod<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TChildEntity : class, IEntity
        where TChildQueryArgs : IQueryArgs
    {
        protected override IChildQueryRepository<TEntity, TKey, TChildEntity, TChildQueryArgs> Repository { get; }

        public GetChildrenListMethod(IAbpLazyServiceProvider serviceProvider,
                                  IChildQueryRepository<TEntity, TKey, TChildEntity, TChildQueryArgs> repository) : base(serviceProvider, repository)
        {
            Repository = repository;
        }

        protected virtual async Task<PaginationResult<TChildEntity>> GetChildrenByQueryAsync(TEntity entity, TChildQueryArgs args)
        {
            if (Repository is IChildQueryAvaliableRepository<TEntity, TKey, TChildEntity, TChildQueryArgs> childQueryAvaliable)
            {
                var requesterId = ServiceProvider.GetRequiredService<ICurrentUser>().Id;
                return await childQueryAvaliable.GetAvaliableChildrenAsync(requesterId, entity, args);
            }
            else
            {
                return await Repository.GetChildrenAsync(entity, args);
            }
        }

        public virtual async Task<PaginationDto<TChildOutputListItemDto>> GetChildrenListAsync(TKey id, TChildQueryArgs input)
        {
            await CheckPolicyAsync();
            var entity = await GetByIdAsync(id);
            await CheckRightsAsync(entity);
            PaginationResult<TChildEntity> pagination = await Repository.GetChildrenAsync(entity, input);
            if (Authorization is IAuthorizationManyChecker<TChildEntity> manyChecker)
            {
                await manyChecker.CheckRightsManyAsync(pagination);
            }
            var dtos = new PaginationDto<TChildOutputListItemDto>();
            foreach (var item in pagination)
            {
                var dto = await MapAsync<TChildEntity, TChildOutputListItemDto>(item);
                dtos.Items.Add(dto);
            }
            return dtos;
        }
    }
}