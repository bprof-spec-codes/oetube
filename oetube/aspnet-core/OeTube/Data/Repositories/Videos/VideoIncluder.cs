using OeTube.Domain.Entities.Videos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Repositories.Videos
{
    public class VideoIncluder : Includer<Video>, ITransientDependency
    {
        protected override IEnumerable<string> GetNavigationProperties()
        {
            yield return nameof(Video.AccessGroups);
            yield return nameof(Video.Resolutions);
        }
    }
}