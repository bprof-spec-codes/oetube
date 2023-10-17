using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Extensions;
using OeTube.Domain.Repositories;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.VirtualFileSystem;

namespace OeTube.Application
{

    public abstract class CreatorCrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        : CrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TKey>, IMayHaveCreator
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
    {
        protected CreatorCrudAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }
 
        protected async virtual Task<TEntity> GetEntityByIdWithCheckOwnerAndUpdatePolicyAsync(TKey id)
        {
            await CheckUpdatePolicyAsync();
            var entity = await GetEntityByIdAsync(id);
            CurrentUser.CheckCreator(entity);
            return entity;
        }
        public async override Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input)
        {
            var entity = await GetEntityByIdWithCheckOwnerAndUpdatePolicyAsync(id);
            await MapToEntityAsync(input, entity);
            await Repository.UpdateAsync(entity, autoSave: true);

            return await MapToGetOutputDtoAsync(entity);
        }
        public async override Task DeleteAsync(TKey id)
        {
            await CheckDeletePolicyAsync();
            var entity=await GetEntityByIdAsync(id);
            CurrentUser.CheckCreator(entity);
            await DeleteByIdAsync(id);
        }
    
    }
}
