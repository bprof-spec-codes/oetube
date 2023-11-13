using Microsoft.AspNetCore.Mvc;
using OeTube.Application.AuthorizationCheckers;
using OeTube.Application.Dtos;
using OeTube.Application.Dtos.Groups;
using OeTube.Application.Dtos.Videos;
using OeTube.Application.Methods;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths.VideoFiles;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application
{

    public class VideoAppService : IApplicationService,ITransientDependency
    {
        private readonly VideoMethodFactory _factory;
        private readonly Type _creatorAuth = typeof(CreatorChecker);
        private readonly Type _accessAuth = typeof(VideoAccessChecker);

        public VideoAppService(VideoMethodFactory videoMethodFactory)
        {
            _factory = videoMethodFactory;
        }

        public async Task<VideoUploadStateDto> StartUploadAsync(StartVideoUploadDto input)
        {
            return await _factory.CreateCreateMethod<StartVideoUploadDto, VideoUploadStateDto>()
                                 .CreateAsync(input);
        }
        public async Task<VideoUploadStateDto> ContinueUploadAsync(Guid id, ContinueVideoUploadDto input)
        {
            return await _factory.CreateUpdateMethod<ContinueVideoUploadDto, VideoUploadStateDto>()
                                 .SetAuthorizationAndPolicy(_creatorAuth)
                                 .UpdateAsync(id, input);
        }
        public async Task<VideoDto> GetAsync(Guid id)
        {
            return await _factory.CreateGetMethod<VideoDto>()
                                 .SetAuthorizationAndPolicy(_accessAuth)
                                 .GetAsync(id);
        }
        public async Task<PaginationDto<VideoListItemDto>> GetListAsync(VideoQueryDto input)
        {
            return await _factory.CreateGetListMethod<VideoListItemDto>()
                                 .GetListAsync(input);
        }
        public async Task<PaginationDto<GroupListItemDto>> GetAccessGroupsAsync(Guid id, GroupQueryDto input)
        {
            return await _factory.CreateGetChildrenListMethod<Group, IGroupQueryArgs, GroupListItemDto>()
                                 .SetAuthorizationAndPolicy(_accessAuth)
                                 .GetChildrenListAsync(id, input);
        }
        public async Task<VideoIndexImagesDto> GetIndexImagesAsync(Guid id)
        {
            return await _factory.CreateGetMethod<VideoIndexImagesDto>()
                                 .SetAuthorizationAndPolicy(_creatorAuth)
                                 .GetAsync(id);
        }
        public async Task<VideoDto> UpdateAsync(Guid id,UpdateVideoDto input)
        {
            return await _factory.CreateUpdateMethod<UpdateVideoDto, VideoDto>()
                                 .SetAuthorizationAndPolicy(_creatorAuth)
                                 .UpdateAsync(id, input);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _factory.CreateDeleteMethod()
                          .SetAuthorizationAndPolicy(_creatorAuth)
                          .DeleteAsync(id);
        }

        [HttpGet("api/src/video/{id}/{width}x{height}/{segment}.ts")]
        public async Task<IRemoteStreamContent> GetHlsSegmentAsync(Guid id, int width, int height, int segment)
        {
            return await _factory.CreateGetFileMethod<HlsSegmentPath>()
                                 .SetAuthorizationAndPolicy(_accessAuth)
                                 .SetFileName($"{width}x{height}_{segment}")
                                 .GetFileAsync(id, new HlsSegmentPath(id, new Resolution(width, height), segment));
        }

        [HttpGet("api/src/video/{id}/{width}x{height}/list.m3u8")]
        public async Task<IRemoteStreamContent?> GetHlsListAsync(Guid id, int width, int height)
        {
            return await _factory.CreateGetFileMethod<HlsListPath>()
                                 .SetAuthorizationAndPolicy(_accessAuth)
                                 .SetFileName($"{width}x{height}")
                                 .GetFileAsync(id, new HlsListPath(id, new Resolution(width, height)));
        }

        [HttpGet("api/src/video/{id}/index_image")]
        public async Task<IRemoteStreamContent?> GetIndexImageAsync(Guid id)
        {
            return await _factory.CreateGetFileMethod<SelectedFramePath>()
                                 .SetAuthorizationAndPolicy(_accessAuth)
                                 .SetFileName("index_image")
                                 .GetFileAsync(id, new SelectedFramePath(id));
        }

        [HttpGet("api/src/video/{id}/index_image/{index}")]
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