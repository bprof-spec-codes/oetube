using OeTube.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace OeTube.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class OeTubeController : AbpControllerBase
{
    protected OeTubeController()
    {
        LocalizationResource = typeof(OeTubeResource);
    }
}
