namespace OeTube.Application.Dtos.Videos
{
    public class UpdateAccessGroupsDto
    {
        public List<Guid> AccessGroups { get; set; } = new List<Guid>();
    }
}