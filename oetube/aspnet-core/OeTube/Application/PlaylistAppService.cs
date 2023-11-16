using Microsoft.AspNetCore.Mvc;
using OeTube.Application.AuthorizationCheckers;
using OeTube.Application.Dtos;
using OeTube.Application.Dtos.Playlists;
using OeTube.Application.Dtos.Videos;
using OeTube.Application.Methods;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths.ImageFiles;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application
{
    public class PlaylistAppService : IApplicationService, ITransientDependency
    {
        private readonly PlaylistMethodFactory _factory;
        private readonly Type _creatorAuth = typeof(CreatorChecker);
        private readonly Type _accessAuth = typeof(PlaylistAccessChecker);

        public PlaylistAppService(PlaylistMethodFactory factory)
        {
            _factory = factory;
        }

        public async Task<PlaylistDto> GetAsync(Guid id)
        {
            return await _factory.CreateGetMethod<PlaylistDto>()
                                 .SetAuthorizationAndPolicy(_accessAuth)
                                 .GetAsync(id);
        }

        public async Task<PaginationDto<PlaylistItemDto>> GetListAsync(PlaylistQueryDto input)
        {
            return await _factory.CreateGetListMethod<PlaylistItemDto>()
                                .SetAuthorizationAndPolicy(_accessAuth)
                                 .GetListAsync(input);
        }

        public async Task<PlaylistDto> CreateAsync(CreateUpdatePlaylistDto input)
        {
            return await _factory.CreateCreateMethod<CreateUpdatePlaylistDto, PlaylistDto>()
                                 .CreateAsync(input);
        }
        public async Task<PlaylistDto> UpdateAsync(Guid id,CreateUpdatePlaylistDto input)
        {
            return await _factory.CreateUpdateMethod<CreateUpdatePlaylistDto, PlaylistDto>()
                                 .SetAuthorizationAndPolicy(_creatorAuth)
                                 .UpdateAsync(id, input);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _factory.CreateDeleteMethod()
                          .SetAuthorizationAndPolicy(_creatorAuth)
                          .DeleteAsync(id);
        }

        public async Task UploadDefaultImageAsync(IRemoteStreamContent input)
        {
            await _factory.CreateUploadDefaultFileMethod<IDefaultImageUploadHandler>()
                          .UploadFile(input);
        }

        public async Task<PaginationDto<VideoListItemDto>> GetVideosAsync(Guid id, VideoQueryDto input)
        {
            var videos = await _factory.CreateGetChildrenListMethod<Video, IVideoQueryArgs, VideoListItemDto>()
                                     .GetChildrenListAsync(id, input);
            foreach (var item in videos.Items)
            {
                item.PlaylistId = id;
            }
            return videos;
        }

        [HttpGet("api/src/playlist/{id}/image")]
        public async Task<IRemoteStreamContent> GetImageAsync(Guid id)
        {
            return await _factory.CreateGetDefaultFileMethod<SourceImagePath>()
                                 .SetFileName("image")
                                 .GetFileAsync(id, new SourceImagePath(id));
        }

        [HttpGet("api/src/playlist/{id}/thumbnail-image")]
        public async Task<IRemoteStreamContent> GetThumbnailImageAsync(Guid id)
        {
            return await _factory.CreateGetDefaultFileMethod<ThumbnailImagePath>()
                                 .SetFileName("thumbnail_image")
                                 .GetFileAsync(id, new ThumbnailImagePath(id));
        }
    }

    public class PlaylistMethodFactory : ApplicationMethodFactory<IPlaylistRepository, Playlist, Guid, IPlaylistQueryArgs>, ITransientDependency
    {
        public PlaylistMethodFactory(IPlaylistRepository repository, IAbpLazyServiceProvider serviceProvider, IFileContainerFactory fileContainerFactory) : base(repository, serviceProvider, fileContainerFactory)
        {
        }
    }
}