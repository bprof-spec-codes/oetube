namespace OeTube.Application.Dtos.Groups
{
    public class ModifyMembersDto
    {
        public List<Guid> Members { get; set; } = new List<Guid>();
    }
}
