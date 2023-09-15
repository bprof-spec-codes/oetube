using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace OeTube;

[Dependency(ReplaceServices = true)]
public class OeTubeBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "OeTube";
}
