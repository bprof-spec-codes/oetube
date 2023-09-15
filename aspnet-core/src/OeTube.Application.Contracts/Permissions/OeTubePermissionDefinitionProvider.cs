using OeTube.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace OeTube.Permissions;

public class OeTubePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(OeTubePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(OeTubePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OeTubeResource>(name);
    }
}
