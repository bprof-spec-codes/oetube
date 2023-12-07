using System.Collections;

namespace OeTube.Domain.Repositories
{
    public class PaginationResult<T> : IEnumerable<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Take { get; set; }
        public int Skip { get; set; }
        public int TotalCount { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}