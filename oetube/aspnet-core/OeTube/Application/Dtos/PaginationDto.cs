namespace OeTube.Application.Dtos
{
    public class PaginationDto<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Skip { get; set; }
        public int Take { get; set; }
        public int TotalCount { get; set; }
        public int Count => Items.Count;
    }
}