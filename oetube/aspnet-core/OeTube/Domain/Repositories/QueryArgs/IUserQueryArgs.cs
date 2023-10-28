namespace OeTube.Domain.Repositories.QueryArgs
{
    public interface IUserQueryArgs : IQueryArgs
    {
        string? Name { get; set; }
        string? EmailDomain { get; set; }
        DateTime? CreationTimeMin { get; set; }
        DateTime? CreationTimeMax { get; set; }
    }
}