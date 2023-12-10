namespace OeTube.Domain.Repositories.QueryArgs
{
    public class Pagination
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
    public interface IQueryArgs
    {
        public Pagination? Pagination { get; set; }
        public string? Sorting { get; set; }
    }

    public class QueryArgs : IQueryArgs
    {
        public Pagination? Pagination { get; set; }
        public virtual string? Sorting { get; set; }
    }
}