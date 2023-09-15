using OeTube.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace OeTube.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(OeTubeEntityFrameworkCoreModule),
    typeof(OeTubeApplicationContractsModule)
    )]
public class OeTubeDbMigratorModule : AbpModule
{
}
