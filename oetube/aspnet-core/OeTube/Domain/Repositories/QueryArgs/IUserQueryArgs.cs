namespace OeTube.Domain.Repositories.QueryArgs
{
    public interface IUserQueryArgs : IQueryArgs
    {
        string? Name { get; set; }
        string? EmailDomain { get; set; }
        DateTime? CreationTimeMin { get; set; }
        DateTime? CreationTimeMax { get; set; }
    }

    public class UserQueryArgs : QueryArgs, IUserQueryArgs
    {
        public virtual string? Name { get; set; }
        public virtual string? EmailDomain { get; set; }
        public virtual DateTime? CreationTimeMin { get; set; }
        public virtual DateTime? CreationTimeMax { get; set; }
    }
}