namespace OeTube.Domain
{
    public class PaginationResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
    }
}
