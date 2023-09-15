using OeTube.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace OeTube;

[DependsOn(
    typeof(OeTubeEntityFrameworkCoreTestModule)
    )]
public class OeTubeDomainTestModule : AbpModule
{

}
