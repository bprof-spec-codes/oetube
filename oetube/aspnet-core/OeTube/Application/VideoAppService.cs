﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OeTube.Application.AuthorizationCheckers;
using OeTube.Application.Dtos;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.Playlists;
using OeTube.Application.Dtos.Videos;
using OeTube.Application.Methods;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths.VideoFiles;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application
{
    public class VideoAppService : IApplicationService, ITransientDependency
    {
        private readonly VideoMethodFactory _factory;
        private readonly Type _creatorAuth = typeof(CreatorChecker);
        private readonly Type _accessAuth = typeof(VideoAccessChecker);
        private readonly Type _creatorOrAdminAuth = typeof(CreatorOrAdminChecker);

        public VideoAppService(VideoMethodFactory videoMethodFactory)
        {
            _factory = videoMethodFactory;
        }

        [Authorize]
        [SwaggerOperation(description: "Feltölti a forrásfájlt és ellenőrzés után a szerver kiosztja az átméretezési feladatokat a kliensnek. Csak bejelentkezett felhasználó tölthet fel és csak mp4 formátum támogatott.")]
        public async Task<VideoUploadStateDto> StartUploadAsync(StartVideoUploadDto input)
        {
            return await _factory.CreateCreateMethod<StartVideoUploadDto, VideoUploadStateDto>()
                                 .CreateAsync(input);
        }

        [Authorize]
        [SwaggerOperation(description: "Feltölti a kliens által átméretezett fájlt az adott id-jű Video-hoz és leellenörzi a tartalmát. Ha az összes kiosztott feladat teljesült a szerver elkezdi feldolgozni a feltöltéseket. Csak a Video létrehozója folytathatja a feltöltést és csak mp4 formátum támogatott.")]
        public async Task<VideoUploadStateDto> ContinueUploadAsync(Guid id, ContinueVideoUploadDto input)
        {
            return await _factory.CreateUpdateMethod<ContinueVideoUploadDto, VideoUploadStateDto>()
                                 .SetAuthorizationAndPolicy(_creatorAuth)
                                 .UpdateAsync(id, input);
        }

        [SwaggerOperation(description: "Lekérdez egy Video-t id alapján és ellenőrzi, hogy a kérelmezőnek van-e hozzáférése.")]
        public async Task<VideoDto> GetAsync(Guid id)
        {
            return await _factory.CreateGetMethod<VideoDto>()
                                 .SetAuthorizationAndPolicy(_accessAuth)
                                 .GetAsync(id);
        }
 
        [SwaggerOperation(description:"Pagináltan lekérdezi az összes kérelmezőnek elérhető Video-t a megadott keresési argumentumok alapján.")]
        public async Task<PaginationDto<VideoListItemDto>> GetListAsync(VideoQueryDto input)
        {
            return await _factory.CreateGetListMethod<VideoListItemDto>()
                                 .GetListAsync(input);
        }

        [SwaggerOperation(description:"Lekérdezi pagináltan az összes adott id-jű Video-hoz tartozó Group-t a keresési argumentumok alapján és ellenörzi, hogy a kérelmezőnek van-e hozzáférése.")]
        public async Task<PaginationDto<GroupListItemDto>> GetAccessGroupsAsync(Guid id, GroupQueryDto input)
        {
            return await _factory.CreateGetChildrenListMethod<Group, IGroupQueryArgs, GroupListItemDto>()
                                 .SetAuthorizationAndPolicy(_accessAuth)
                                 .GetChildrenListAsync(id, input);
        }

        [SwaggerOperation(description:"Lekérdezi az adott id-jű Video-hoz tartozó indexképeket. Csak a létrehozója fér hozzá.")]
        public async Task<VideoIndexImagesDto> GetIndexImagesAsync(Guid id)
        {
            return await _factory.CreateGetMethod<VideoIndexImagesDto>()
                                 .SetAuthorizationAndPolicy(_creatorAuth)
                                 .GetAsync(id);
        }

        [Authorize]
        [SwaggerOperation(description:"Módosit egy Video-t. Csak a létrehozója hajthatja végre.")]
        public async Task<VideoDto> UpdateAsync(Guid id, UpdateVideoDto input)
        {
            return await _factory.CreateUpdateMethod<UpdateVideoDto, VideoDto>()
                                 .SetAuthorizationAndPolicy(_creatorAuth)
                                 .UpdateAsync(id, input);
        }

        [Authorize]
        [SwaggerOperation(description:"Kitöröl egy Video-t. Csak a létrehozója hajthatja végre.")]
        public async Task DeleteAsync(Guid id)
        {
            await _factory.CreateDeleteMethod()
                          .SetAuthorizationAndPolicy(_creatorOrAdminAuth)
                          .DeleteAsync(id);
        }

        [HttpGet("api/src/video/{id}/{width}x{height}/{segment}.ts")]
        [SwaggerOperation(description:"Lekéri az adott id-jű Video-hoz tartozó HLS szegmenst felbontás(widthxheight) és sorszám(segment) alapján. Ellenőrzi, hogy a kérelmezőnek van-e hozzáférése.")]
        public async Task<IRemoteStreamContent> GetHlsSegmentAsync(Guid id, int width, int height, int segment)
        {
            return await _factory.CreateGetFileMethod<HlsSegmentPath>()
                                 .SetAuthorizationAndPolicy(_accessAuth)
                                 .SetFileName($"{width}x{height}_{segment}")
                                 .GetFileAsync(id, new HlsSegmentPath(id, new Resolution(width, height), segment));
        }

        [HttpGet("api/src/video/{id}/{width}x{height}/list.m3u8")]
        [SwaggerOperation(description:"Lekéri az adott id-jű Video-hoz tartozó HLS listát felbontás(widthxheight) alapján. Ellenőrzi, hogy a kérelmezőnek van-e hozzáférése.")]
        public async Task<IRemoteStreamContent?> GetHlsListAsync(Guid id, int width, int height)
        {
            return await _factory.CreateGetFileMethod<HlsListPath>()
                                 .SetAuthorizationAndPolicy(_accessAuth)
                                 .SetFileName($"{width}x{height}")
                                 .GetFileAsync(id, new HlsListPath(id, new Resolution(width, height)));
        }

        [HttpGet("api/src/video/{id}/index_image")]
        [SwaggerOperation(description:"Lekéri az adott id-jű Video-hoz tartozó, beállított indexképet. Ellenőrzi, hogy a kérelmezőnek van-e hozzáférése.")]
        public async Task<IRemoteStreamContent?> GetIndexImageAsync(Guid id)
        {
            return await _factory.CreateGetFileMethod<SelectedFramePath>()
                                 .SetAuthorizationAndPolicy(_accessAuth)
                                 .SetFileName("index_image")
                                 .GetFileAsync(id, new SelectedFramePath(id));
        }

        [HttpGet("api/src/video/{id}/index_image/{index}")]
        [SwaggerOperation(description:"Lekéri az adott id-jű Video-hoz tartozó indexképet index alapján. Csak a Video létrehozója fér hozzá.")]
        public async Task<IRemoteStreamContent?> GetIndexImageByIndexAsync(Guid id, int index)
        {
            return await _factory.CreateGetFileMethod<FramePath>()
                                  .SetAuthorizationAndPolicy(_creatorAuth)
                                  .SetFileName($"index_image_{index}")
                                  .GetFileAsync(id, new FramePath(id, index));
        }
    }

    public class VideoMethodFactory : ApplicationMethodFactory<IVideoRepository, Video, Guid, IVideoQueryArgs>, ITransientDependency
    {
        public VideoMethodFactory(IVideoRepository repository, IAbpLazyServiceProvider serviceProvider, IFileContainerFactory fileContainerFactory) : base(repository, serviceProvider, fileContainerFactory)
        {
        }
    }
}