using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data;

/* This is used if database provider does't define
 * IOeTubeDbSchemaMigrator implementation.
 */
public class NullOeTubeDbSchemaMigrator : IOeTubeDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
