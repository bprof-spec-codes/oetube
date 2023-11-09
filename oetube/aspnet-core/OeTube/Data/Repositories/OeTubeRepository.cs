using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict;

namespace OeTube.Data.Repositories
{


    public abstract class OeTubeRepository<TEntity, TKey, TIncluder, TFilter, TQueryArgs> : EfCoreRepository<OeTubeDbContext, TEntity, TKey>, ICustomRepository<TEntity, TKey, TQueryArgs>
       where TEntity : class, IEntity<TKey>
       where TQueryArgs : IQueryArgs
       where TFilter : IFilter<TEntity, TQueryArgs>
       where TIncluder : IIncluder<TEntity>
    {
        protected OeTubeRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

   
        public override async Task<IQueryable<TEntity>> WithDetailsAsync()
        {
            return Include<TEntity, TIncluder>(await GetQueryableAsync<TEntity>(),true);
        }

        public async Task<PaginationResult<TEntity>> GetListAsync(TQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await CreateListAsync<TEntity, TIncluder, TFilter, TQueryArgs>
                (await GetQueryableAsync<TEntity>(), args, includeDetails, cancellationToken);
        }

        public async Task<List<TEntity>> GetManyAsync(IEnumerable<TKey> ids, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await GetManyAsync<TEntity, TKey, TIncluder>(ids, includeDetails, cancellationToken);
        }
        protected async Task<IQueryable<T>> GetQueryableAsync<T>()
            where T : class, IEntity
        {
            return (await GetDbContextAsync()).Set<T>();
        }
        protected async Task<DbSet<TSetEntity>> GetDbSetAsync<TSetEntity>()
        where TSetEntity : class, IEntity
        {
            return (await GetDbContextAsync()).Set<TSetEntity>();
        }
        protected async Task<bool> HasPlaylistAccessAsync(Guid? requesterId,Playlist playlist)
        {
            var videos = await GetChildEntitiesAsync<Playlist, Guid, VideoItem, Video, Guid>(playlist);
            foreach (var item in videos)
            {
                if (await HasVideoAccessAsync(requesterId, item))
                {
                    return true;
                }
            }
            return false;
        }
        protected async Task<bool> HasVideoAccessAsync(Guid? requesterId, Video video)
        {
            if (requesterId is null)
            {
                return video.Access == AccessType.Public;
            }
            else
            {
                if (video.Access == AccessType.Public || requesterId == video.CreatorId)
                {
                    return true;
                }
                if (video.Access == AccessType.Private)
                {
                    return false;
                }
                if (video.AccessGroups.Count == 0)
                {
                    return false;
                }
                var groups = await GetChildEntitiesAsync<Video, Guid, AccessGroup, Group, Guid>(video);
                var members = await GetMembersAsync();
                var result = (from @group in groups
                              join member in members
                on @group.Id equals member.GroupId
                              select @group.Id).FirstOrDefault();

                return result != default;
            }
        }

        protected async Task<IQueryable<Playlist>> GetAvaliablePlaylistsAsync(Guid? requesterId, IQueryable<Playlist>? playlists = null)
        {
            playlists ??= await GetQueryableAsync<Playlist>();

            var videos = from videoItem in await GetQueryableAsync<VideoItem>()
                         join video in await GetAvaliableVideosAsync(requesterId)
                         on videoItem.VideoId equals video.Id
                         select videoItem;


            var result = (from playlist in playlists
                          join videoItem in videos
                          on playlist.Id equals videoItem.PlaylistId
                          select playlist).Distinct();
            return result;
        }
        protected async Task<IQueryable<Video>> GetAvaliableVideosAsync(Guid? requesterId, IQueryable<Video>? videoQueryable = null)
        {
            videoQueryable ??= await GetQueryableAsync<Video>();
            videoQueryable = from video in videoQueryable
                             where video.IsUploadCompleted
                             select video;

            if (requesterId is null)
            {
                return from video in videoQueryable
                       where video.Access == AccessType.Public
                       select video;
            }

            var accessGroups = await GetQueryableAsync<AccessGroup>();
            var members = await GetMembersAsync();

            var joined = (from member in members
                          where member.UserId == requesterId
                          join accessGroup in accessGroups
                          on member.GroupId equals accessGroup.GroupId
                          select accessGroup.VideoId).Distinct();

            var groupAccess = from video in videoQueryable
                              where video.Access == AccessType.Group
                              join id in joined
                              on video.Id equals id
                              select video;

            var trivialAccess = from video in videoQueryable
                                where video.Access == AccessType.Public || video.CreatorId == requesterId
                                select video;

            return groupAccess.Concat(trivialAccess).Distinct();
        }
        protected async Task<IQueryable<Member>> GetMembersAsync()
        {
            var domainMembers = from emailDomain in await GetQueryableAsync<EmailDomain>()
                                join user in await GetQueryableAsync<OeTubeUser>()
                                on emailDomain.Domain equals user.EmailDomain
                                select new Member(emailDomain.GroupId, user.Id);

            return (await GetQueryableAsync<Member>()).Concat(domainMembers).Distinct();
        }

        protected async Task<IQueryable<TReferencedEntity>> GetChildEntitiesAsync
        <TParentEntity, TParentKey, TChildEntity, TReferencedEntity, TReferencedKey>
        (TParentEntity parentEntity)
        where TParentEntity : class, IEntity<TParentKey>
        where TChildEntity : class, IEntity, IHasForeignKey<TParentEntity, TParentKey>, IHasForeignKey<TReferencedEntity, TReferencedKey>
        where TReferencedEntity : class, IEntity<TReferencedKey>
        {

            var result = from child in await GetQueryableAsync<TChildEntity>()
                         where (child as IHasForeignKey<TParentEntity, TParentKey>).ForeignKey!.Equals(parentEntity.Id)
                         join referenced in await GetQueryableAsync<TReferencedEntity>()
                         on (child as IHasForeignKey<TReferencedEntity, TReferencedKey>).ForeignKey equals referenced.Id
                         select referenced;
            return result;
        }
        protected async Task<IQueryable<TCreatedEntity>> GetCreatedEntitiesAsync<TCreatedEntity>(Guid creatorID)
        where TCreatedEntity : class, IEntity, IMayHaveCreator
        {
            var result = from entity in await GetQueryableAsync<TCreatedEntity>()
                         where entity.CreatorId != creatorID
                         select entity;
            return result;
        }

        protected async Task<TCreator?> GetCreatorAsync<TCreatedEntity, TCreator, TCreatorIncluder>(TCreatedEntity entity, bool includeDetails = true, CancellationToken cancellationToken = default)
        where TCreatedEntity : class, IEntity, IMayHaveCreator
        where TCreator : class, IEntity<Guid>
        where TCreatorIncluder : IIncluder<TCreator>
        {
            if (entity.CreatorId is null) return null;
            return await GetAsync<TCreator, Guid, TCreatorIncluder>(entity.CreatorId.Value, includeDetails, cancellationToken);
        }

        protected async Task<TGetEntity> GetAsync<TGetEntity, TEntityKey, TEntityIncluder>(TEntityKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        where TGetEntity : class, IEntity<TEntityKey>
        where TEntityIncluder : IIncluder<TGetEntity>
        {
            var queryable = Include<TGetEntity, TEntityIncluder>((await GetQueryableAsync<TGetEntity>()).OrderBy(e => e.Id), includeDetails);
            var entity = await queryable.FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
            if (entity is null)
            {
                throw new EntityNotFoundException(typeof(TGetEntity), id);
            }

            return entity;
        }
        protected IQueryable<TIncludedEntity> Include<TIncludedEntity, TEntityIncluder>(IQueryable<TIncludedEntity> queryable, bool includeDetails)
          where TIncludedEntity : class, IEntity
          where TEntityIncluder : IIncluder<TIncludedEntity>
        {
            if (includeDetails)
            {
                var includer = LazyServiceProvider.GetRequiredService<TEntityIncluder>();
                queryable = includer.Include(queryable);
            }
            return queryable;
        }
        protected async Task<PaginationResult<TListEntity>> CreateListAsync<TListEntity>(IQueryable<TListEntity> queryable,
                                                                                        IQueryArgs? args,
                                                                                        CancellationToken cancellationToken)
         where TListEntity : class, IEntity
        {
            if (!await queryable.AnyAsync(cancellationToken))
            {
                return new PaginationResult<TListEntity>();
            }
            try
            {
                queryable = queryable.OrderByIf<TListEntity, IQueryable<TListEntity>>(args?.Sorting is not null, args!.Sorting!);
            }
            catch { }

            int itemPerPage = args?.ItemPerPage ?? 0;
            if (itemPerPage <= 0)
            {
                return new PaginationResult<TListEntity>();
            }
            int totalCount = queryable.Count();
            (int quotient, int remainder) = Math.DivRem(totalCount, itemPerPage);
            int totalPage = remainder > 0 ? quotient + 1 : quotient;

            int page = args?.Page ?? 0;
            page = page >= 0 ? page : 0;
            page = page < totalPage ? page : totalPage - 1;

            queryable = queryable.Skip(page * itemPerPage);
            queryable = queryable.Take(itemPerPage);


            var items = await queryable.ToListAsync(cancellationToken);
            return new PaginationResult<TListEntity>()
            {
                CurrentPage = page,
                PageCount = totalPage,
                Items = items,
                TotalCount = totalCount
            };
        }

        protected async Task<PaginationResult<TListEntity>> CreateListAsync<TListEntity, TEntityIncluder>(IQueryable<TListEntity> queryable,
                                                                                                               IQueryArgs? args,
                                                                                                               bool includeDetails,
                                                                                                               CancellationToken cancellationToken)
            where TListEntity : class, IEntity
            where TEntityIncluder : IIncluder<TListEntity>
        {
            queryable = Include<TListEntity, TEntityIncluder>(queryable, includeDetails);
            return await CreateListAsync(queryable, args, cancellationToken);
        }
        protected async Task<PaginationResult<TListEntity>> CreateListAsync<TListEntity, TEntityIncluder, TEntityFilter, TEntityQueryArgs>
            (IQueryable<TListEntity> queryable, TEntityQueryArgs? args, bool includeDetails = false, CancellationToken cancellationToken = default)
            where TListEntity : class, IEntity
            where TEntityFilter : IFilter<TListEntity, TEntityQueryArgs>
            where TEntityQueryArgs : IQueryArgs
            where TEntityIncluder : IIncluder<TListEntity>
        {
            if (args is not null)
            {
                var filter = LazyServiceProvider.GetRequiredService<TEntityFilter>();
                queryable = filter.FilterQueryable(queryable, args);
            }
            return await CreateListAsync<TListEntity, TEntityIncluder>(queryable, args, includeDetails, cancellationToken);
        }
        protected async Task<List<TManyEntity>> GetManyAsync<TManyEntity, TEntityKey, TEntityIncluder>
        (IEnumerable<TEntityKey> ids, bool includeDetails, CancellationToken cancellationToken)
           where TManyEntity : class, IEntity<TEntityKey>
           where TEntityIncluder : IIncluder<TManyEntity>
        {
            var queryable = (await GetQueryableAsync<TManyEntity>()).Where(e => ids.Contains(e.Id));
            if (includeDetails)
            {
                var includer = LazyServiceProvider.LazyGetRequiredService<TEntityIncluder>();
                queryable = includer.Include(queryable);
            }

            return await queryable.ToListAsync(cancellationToken);
        }

    }
}