namespace OeTube.Domain.Repositories.QueryArgs
{
    public interface IPlaylistQueryArgs : IQueryArgs
    {
        string? Name { get; set; }
        DateTime? CreationTimeMin { get; set; }
        DateTime? CreationTimeMax { get; set; }
        Guid? CreatorId { get; set; }
    }

    public class PlaylistQueryArgs : QueryArgs, IPlaylistQueryArgs
    {
        public virtual string? Name { get; set; }
        public virtual DateTime? CreationTimeMin { get; set; }
        public virtual DateTime? CreationTimeMax { get; set; }
        public Guid? CreatorId { get; set; }
    }
}