using Autofac.Builder;
using OeTube.Domain.Entities.Groups;
using OeTube.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.OeTubeUsers
{
    public class OeTubeUserItemMapper : IObjectMapper<OeTubeUser, OeTubeUserItemDto>, ITransientDependency
    {
        public OeTubeUserItemDto Map(OeTubeUser source)
        {
            return Map(source, new OeTubeUserItemDto());
        }

        public OeTubeUserItemDto Map(OeTubeUser source, OeTubeUserItemDto destination)
        {
            destination.Id = source.Id;
            destination.EmailDomain = source.EmailDomain;
            destination.Name = source.Name;
            destination.RegistrationDate = source.CreationTime;
            return destination;
        }
    }

    public class OeTubeUserItemDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string EmailDomain { get; set; }
    }
}
