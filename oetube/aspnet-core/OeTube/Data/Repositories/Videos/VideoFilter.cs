using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Repositories.QueryArgs;
using System.Linq.Expressions;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Repositories.Videos
{
    public class VideoFilter : Filter<Video, IVideoQueryArgs>, ITransientDependency
    {
        protected override Expression<Func<Video, bool>> GetFilter(IVideoQueryArgs args)
        {
            return video =>video.IsUploadCompleted&&
                (string.IsNullOrEmpty(args.Name) || video.Name.Contains(args.Name)) &&
                (args.CreationTimeMin == null || args.CreationTimeMin <= video.CreationTime) &&
                (args.CreationTimeMax == null || args.CreationTimeMax >= video.CreationTime) &&
                (args.DurationMin == null || args.DurationMin <= video.Duration) &&
                (args.DurationMax == null || args.DurationMax >= video.Duration);
        }
    }
}