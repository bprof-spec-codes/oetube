using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Application.Dtos.OeTubeUsers
{
    public class UserQueryDto : IUserQueryArgs
    {
        public string? Name { get; set; }
        public string? EmailDomain { get; set; }
        public DateTime? CreationTimeMin { get; set; }
        public DateTime? CreationTimeMax { get; set; }
        public int? SkipCount { get; set; }
        public int? MaxResultCount { get; set; }
        public string? Sorting { get; set; }
    }
}