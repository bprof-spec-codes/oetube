namespace OeTube.Domain.Repositories.QueryArgs
{
    public interface IPlaylistQueryArgs : IQueryArgs
    {
        string? Name { get; set; }
        DateTime? CreationTimeMin { get; set; }
        DateTime? CreationTimeMax { get; set; }
    }
}