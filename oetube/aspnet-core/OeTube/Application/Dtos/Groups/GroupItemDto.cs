using OeTube.Domain.Entities.Groups;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.Groups
{
    public class GroupItemMapper : IObjectMapper<Group, GroupItemDto>, ITransientDependency
    {
        public GroupItemDto Map(Group source)
        {
            return Map(source, new GroupItemDto());
        }

        public GroupItemDto Map(Group source, GroupItemDto destination)
        {
            destination.Id = source.Id;
            destination.CreationTime = source.CreationTime;
            destination.CreatorId = source.CreatorId;
            destination.Name = source.Name;
            return destination;
        }
    }
    public class GroupItemDto : EntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
    }
}
