using Microsoft.AspNetCore.Components.Forms;
using OeTube.Application.Methods.CreateMethods;
using OeTube.Application.Methods.DeleteMethods;
using OeTube.Application.Methods.GetListMethods;
using OeTube.Application.Methods.GetMethods;
using OeTube.Application.Methods.UpdateMethods;
using OeTube.Application.AuthorizationCheckers;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods
{



    public class ApplicationMethodFactory<TRepository,TEntity, TKey, TQueryArgs>
        where TEntity : class, IEntity<TKey>
        where TQueryArgs : IQueryArgs
        where TRepository : ICustomRepository<TEntity,TKey,TQueryArgs>
    {
        public ApplicationMethodFactory(TRepository repository, IAbpLazyServiceProvider serviceProvider, IFileContainerFactory fileContainerFactory)
        {
            Repository = repository;
            ServiceProvider = serviceProvider;
            FileContainer = fileContainerFactory.Create<TEntity>();
        }

        protected virtual TRepository Repository { get; }
        protected virtual IAbpLazyServiceProvider ServiceProvider { get; }
        protected virtual IFileContainer FileContainer { get; }


        public virtual GetMethod<TEntity, TKey, TOutputDto> CreateGetMethod<TOutputDto>()
            
        {
            return new GetMethod<TEntity, TKey, TOutputDto>(ServiceProvider,  Repository);
        }

        public virtual GetFileMethod<TEntity, TKey, TInputFilePath> CreateGetFileMethod<TInputFilePath>()
            where TInputFilePath : IFilePath
            
        {
            return new GetFileMethod<TEntity, TKey, TInputFilePath>(ServiceProvider,  Repository, FileContainer);
        }
       
        public virtual GetDefaultFileMethod<TEntity, TKey, TInputFilePath> CreateGetDefaultFileMethod<TInputFilePath>()
        where TInputFilePath : IDefaultFilePath
            
        {
            return new GetDefaultFileMethod<TEntity, TKey, TInputFilePath>(ServiceProvider,  Repository, FileContainer);
        }
        public virtual GetChildrenListMethod<TEntity,TKey,TChildEntity,TChildQueryArgs,TChildOutputListItemDto> 
            CreateGetChildrenListMethod<TChildEntity,TChildQueryArgs, TChildOutputListItemDto>()
        where TChildEntity:class,IEntity
        where TChildQueryArgs:IQueryArgs
        {
            if(Repository is not IChildQueryRepository<TEntity,TKey,TChildEntity,TChildQueryArgs> childQuery)
            {
                throw new NotSupportedException();
            }

            return new GetChildrenListMethod<TEntity, TKey, TChildEntity, TChildQueryArgs, TChildOutputListItemDto>
                (ServiceProvider,  childQuery);
        }

        public virtual GetListMethod<TEntity, TQueryArgs, TOutputListItemDto> CreateGetListMethod<TOutputListItemDto>()
         
        {
            return new GetListMethod<TEntity, TQueryArgs, TOutputListItemDto>(ServiceProvider,  Repository);
        }

        public virtual DeleteMethod<TEntity, TKey> CreateDeleteMethod()
         
        {
            return new DeleteMethod<TEntity, TKey>(ServiceProvider,  Repository);
        }

        public virtual DeleteFileMethod<TEntity, TKey, TInputFilePath> CreateDeleteFileMethod< TInputFilePath>()
         
         where TInputFilePath : IFilePath
        {
            return new DeleteFileMethod<TEntity, TKey, TInputFilePath>(ServiceProvider,  Repository, FileContainer);
        }
        public virtual UploadDefaultFileMethod<TEntity,TFileHandler> CreateUploadDefaultFileMethod<TFileHandler>()
        where TFileHandler:IFileHandler<ByteContent>

        {
            return new UploadDefaultFileMethod<TEntity,TFileHandler>(ServiceProvider);
        }

        public virtual DeleteDefaultFileMethod<TEntity, TKey, TInputFilePath> CreateDeleteDefaultFileMethod< TInputFilePath>()
        
        where TInputFilePath : IDefaultFilePath
        {
            return new DeleteDefaultFileMethod<TEntity, TKey, TInputFilePath>(ServiceProvider,  Repository, FileContainer);
        }

        public virtual CreateMethod<TEntity, TKey, TInputDto, TOutputDto> CreateCreateMethod< TInputDto, TOutputDto>()
        
        {
            return new CreateMethod<TEntity, TKey, TInputDto, TOutputDto>(ServiceProvider,  Repository);
        }
        public virtual UpdateMethod<TEntity, TKey, TInputDto, TOutputDto> CreateUpdateMethod< TInputDto, TOutputDto>()
            
        {
            return new UpdateMethod<TEntity, TKey, TInputDto, TOutputDto>(ServiceProvider,  Repository);
        }

       
    }
}