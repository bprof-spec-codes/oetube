using System;
using System.Collections.Generic;
using System.Text;
using OeTube.Localization;
using Volo.Abp.Application.Services;

namespace OeTube;

/* Inherit your application services from this class.
 */
public abstract class OeTubeAppService : ApplicationService
{
    protected OeTubeAppService()
    {
        LocalizationResource = typeof(OeTubeResource);
    }
}
