using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OeTube.Data;
using Volo.Abp.DependencyInjection;

namespace OeTube.EntityFrameworkCore;

public class EntityFrameworkCoreOeTubeDbSchemaMigrator
    : IOeTubeDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreOeTubeDbSchemaMigrator(
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
