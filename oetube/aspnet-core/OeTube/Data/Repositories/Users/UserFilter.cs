using OeTube.Domain.Repositories.QueryArgs;
using OeTube.Entities;
using System.Linq.Expressions;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Repositories.Users
{
    public class UserFilter : Filter<OeTubeUser, IUserQueryArgs>, ITransientDependency
    {
        protected override Expression<Func<OeTubeUser, bool>> GetFilter(IUserQueryArgs args)
        {
            return user => 
            (string.IsNullOrEmpty(args.Name) || user.Name.ToLower().Contains(args.Name.ToLower())) &&
            (string.IsNullOrEmpty(args.EmailDomain)||user.EmailDomain.ToLower().Contains(args.EmailDomain??"".ToLower()))&&
            (args.CreationTimeMin == null || args.CreationTimeMin <= user.CreationTime) &&
            (args.CreationTimeMax == null || args.CreationTimeMax >= user.CreationTime);
        }
    }
}