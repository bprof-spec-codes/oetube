namespace OeTube.Domain.Repositories.QueryArgs
{
    public interface IQueryArgs
    {
        int? MaxResultCount { get; set; }
        int? SkipCount { get; set; }
        string? Sorting { get; set; }
    }
    public class QueryArgs:IQueryArgs
    {
        public int? MaxResultCount { get; set; }
        public int? SkipCount { get; set; }
        public string? Sorting { get; set; }
    }
}