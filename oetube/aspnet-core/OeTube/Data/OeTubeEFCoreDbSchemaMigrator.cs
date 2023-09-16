using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace OeTube.Data;

public class OeTubeEFCoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public OeTubeEFCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the OeTubeDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<OeTubeDbContext>()
            .Database
            .MigrateAsync();
    }
}
