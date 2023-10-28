namespace OeTube.Domain.Repositories.QueryArgs
{
    public interface IQueryArgs
    {
        int? MaxResultCount { get; set; }
        int? SkipCount { get; set; }
        string? Sorting { get; set; }
    }
}