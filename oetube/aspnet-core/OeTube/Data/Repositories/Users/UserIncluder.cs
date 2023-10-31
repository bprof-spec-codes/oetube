using OeTube.Entities;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Repositories.Users
{
    public class UserIncluder : Includer<OeTubeUser>, ITransientDependency
    {
        protected override IEnumerable<string> GetNavigationProperties()
        {
            return Enumerable.Empty<string>();
        }
    }
}