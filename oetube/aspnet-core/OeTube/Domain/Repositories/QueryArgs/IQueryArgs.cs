namespace OeTube.Domain.Repositories.QueryArgs
{
    public interface IQueryArgs
    {
        int Page { get; set; }
        ItemPerPage ItemPerPage { get; set; }
        string? Sorting { get; set; }
    }
    public enum ItemPerPage
    {
        P10=10,P20=20,P50=50,P100=100
    }
    public class QueryArgs:IQueryArgs
    {
        public virtual int Page { get; set; } = 0;
        public virtual ItemPerPage ItemPerPage { get; set; }
        public virtual string? Sorting { get; set; }
    }
}