namespace OeTube.Domain.Repositories.QueryArgs
{
    public interface IQueryArgs
    {
        int? Page { get; set; }
        int? ItemPerPage { get; set; }
        string? Sorting { get; set; }
    }

    public class QueryArgs:IQueryArgs
    {
        public virtual int? Page { get; set; }
        public virtual int? ItemPerPage { get; set; }
        public virtual string? Sorting { get; set; }
    }
}