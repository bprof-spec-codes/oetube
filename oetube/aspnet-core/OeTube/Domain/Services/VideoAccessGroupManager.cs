using OeTube.Data.Repositories;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.Extensions;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace OeTube.Domain.Services
{
    public class VideoAccessGroupManager : DomainService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IGroupRepository _groupRepository;

        public VideoAccessGroupManager(IVideoRepository videoRepository, IGroupRepository groupRepository)
        {
            _videoRepository = videoRepository;
            _groupRepository = groupRepository;
        }
        public async Task<Video> UpdateAccessGroupsAsync(Video video, IEnumerable<Guid> accessGroups, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var groups = await _groupRepository.GetManyAsSetAsync(accessGroups, false, cancellationToken);
            if (groups.Count != accessGroups.Count())
            {
                throw new EntityNotFoundException(typeof(Video));
            }
            await _videoRepository.UpdateAccessGroupsAsync(video, groups, autoSave, cancellationToken);
            return video;
        }
    }
}
