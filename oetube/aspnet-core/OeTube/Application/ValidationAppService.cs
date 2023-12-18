using OeTube.Application.Dtos.Validations;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application
{
    public class ValidationAppService:IApplicationService,ITransientDependency
    {
        public ValidationsDto Get()
        {
            return new ValidationsDto();
        }
    }
}
