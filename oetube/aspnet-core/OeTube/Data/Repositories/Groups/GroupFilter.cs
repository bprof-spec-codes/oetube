using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories.QueryArgs;
using System.Linq.Expressions;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Repositories.Groups
{
    public class GroupFilter : Filter<Group, IGroupQueryArgs>, ITransientDependency
    {
        protected override Expression<Func<Group, bool>> GetFilter(IGroupQueryArgs args)
        {
            return group =>
                 (args.CreatorId==null||group.CreatorId==args.CreatorId)&&
                 (string.IsNullOrWhiteSpace(args.Name) || group.Name.ToLower().Contains(args.Name.ToLower())) &&
                 (args.CreationTimeMin == null || args.CreationTimeMin <= group.CreationTime) &&
                 (args.CreationTimeMax == null || args.CreationTimeMax >= group.CreationTime);
        }
    }
}