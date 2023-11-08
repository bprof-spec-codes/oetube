namespace OeTube.Application.Dtos
{
    public class PaginationDto<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
        public int Count => Items.Count;
    }
}
