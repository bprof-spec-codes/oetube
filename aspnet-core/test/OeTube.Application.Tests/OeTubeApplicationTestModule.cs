using Volo.Abp.Modularity;

namespace OeTube;

[DependsOn(
    typeof(OeTubeApplicationModule),
    typeof(OeTubeDomainTestModule)
    )]
public class OeTubeApplicationTestModule : AbpModule
{

}
