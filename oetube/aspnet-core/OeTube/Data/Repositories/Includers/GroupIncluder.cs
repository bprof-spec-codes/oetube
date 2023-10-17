using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories.Extensions;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Repositories.Includers
{
    [ExposeServices(typeof(IIncluder<Group>))]
    public class GroupIncluder : Includer<Group>, ITransientDependency
    {
        protected override IEnumerable<string> GetNavigationProperties()
        {
            yield return nameof(Group.EmailDomains);
            yield return nameof(Group.Members);
        }
    }

}
