using OeTube.Application.Caches;
using OeTube.Application.Caches.Composite;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Repositories;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.Playlists
{
    public class CreateUpdatePlaylistMapper : AsyncObjectMapper<CreateUpdatePlaylistDto, Playlist>, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentUser _currentUser;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IImageUploadHandler _imageUploadHandler;
        private readonly IVideoRepository _videoRepository;
        private readonly PlaylistCacheService _cacheService;

        public CreateUpdatePlaylistMapper(IGuidGenerator guidGenerator,
                                          ICurrentUser currentUser,
                                          IPlaylistRepository playlistRepository,
                                          IImageUploadHandler imageUploadHandler,
                                          IVideoRepository videoRepository,
                                          PlaylistCacheService cacheService)
        {
            _guidGenerator = guidGenerator;
            _currentUser = currentUser;
            _playlistRepository = playlistRepository;
            _imageUploadHandler = imageUploadHandler;
            _videoRepository = videoRepository;
            _cacheService = cacheService;
        }

        public override async Task<Playlist> MapAsync(CreateUpdatePlaylistDto source)
        {
            var id = _guidGenerator.Create();
            var playlist = new Playlist(id, source.Name, _currentUser.Id);
            return await MapAsync(source, playlist);
        }

        public override async Task<Playlist> MapAsync(CreateUpdatePlaylistDto source, Playlist destination)
        {
            destination.SetName(source.Name)
                       .SetDescription(source.Description);

            Dictionary<Guid, int> order = new();
            for (int i = 0; i < source.Items.Count; i++)
            {
                order.Add(source.Items[i], i);
            }


            var videos = await _videoRepository.GetManyAsync(source.Items);
            videos.Sort((x, y) => order[x.Id].CompareTo(order[y.Id]));
            

            await _playlistRepository.UpdateChildrenAsync(destination, videos);
            if (source.Image is not null)
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(source.Image);
                await _imageUploadHandler.HandleFileAsync<Playlist>(new ImageUploadHandlerArgs(destination.Id, content));
            }

            return destination;
        }
    }

    public class CreateUpdatePlaylistDto
    {
        [Required]
        [StringLength(PlaylistConstants.NameMaxLength, MinimumLength = PlaylistConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;

        [StringLength(PlaylistConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public List<Guid> Items { get; set; } = new List<Guid>();
        public IRemoteStreamContent? Image { get; set; }
    }
}