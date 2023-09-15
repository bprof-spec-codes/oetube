using Volo.Abp.Settings;

namespace OeTube.Settings;

public class OeTubeSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(OeTubeSettings.MySetting1));
    }
}
