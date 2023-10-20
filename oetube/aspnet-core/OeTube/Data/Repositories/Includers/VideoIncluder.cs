using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.Extensions;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Repositories.Includers
{
    [ExposeServices(typeof(IIncluder<Video>))]
    public class VideoIncluder : Includer<Video>, ITransientDependency
    {
        protected override IEnumerable<string> GetNavigationProperties()
        {
            yield return nameof(Video.AccessGroups);
            yield return nameof(Video.Resolutions);
        }
    }

}
