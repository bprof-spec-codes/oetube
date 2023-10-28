using OeTube.Domain.Entities.Groups;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data.Repositories.Groups
{
    public class GroupIncluder : Includer<Group>, ITransientDependency
    {
        protected override IEnumerable<string> GetNavigationProperties()
        {
            yield return nameof(Group.EmailDomains);
            yield return nameof(Group.Members);
        }
    }
}