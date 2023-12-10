using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Application.Dtos.Groups
{
    public class GroupQueryDto : QueryDto, IGroupQueryArgs
    {
        public string? Name { get; set; }
        public DateTime? CreationTimeMin { get; set; }
        public DateTime? CreationTimeMax { get; set; }
        public Guid? CreatorId { get; set; }
    }
}