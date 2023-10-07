using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;

namespace OeTube.Services.Dtos.OeTubeUsers
{
    public class OeTubeUserMapper : IObjectMapper<OeTubeUser, OeTubeUserDto>, ITransientDependency
    {
        public OeTubeUserDto Map(OeTubeUser source)
        {
            return Map(source, new OeTubeUserDto());
        }

        public OeTubeUserDto Map(OeTubeUser source, OeTubeUserDto destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.RegistrationDate = source.CreationTime;
            return destination;
        }
    }

    public class OeTubeUserDto:EntityDto<Guid>
    {
        public string Name { get; set; }
        public string? AboutMe { get; set; }
        public string EmailDomain { get; set; }
        public DateTime RegistrationDate { get; set; }


    }
}
