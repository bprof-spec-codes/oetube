using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Application.Dtos.Groups
{
    public class GroupQueryDto : IGroupQueryArgs
    {
        public string? Name { get; set; }
        public DateTime? CreationTimeMin { get; set; }
        public DateTime? CreationTimeMax { get; set; }
        public int? ItemPerPage { get; set; }
        public int? Page { get; set; }
        public string? Sorting { get; set; }
    }
}