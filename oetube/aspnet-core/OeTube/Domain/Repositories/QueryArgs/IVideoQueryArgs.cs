namespace OeTube.Domain.Repositories.QueryArgs
{
    public interface IVideoQueryArgs : IQueryArgs
    {
         string? Name { get; set; }
         DateTime? CreationTimeMin { get; set; }
         DateTime? CreationTimeMax { get; set; }
         TimeSpan? DurationMin { get; set; }
         TimeSpan? DurationMax { get; set; }
         Guid? CreatorId { get; set; }
    }
    public class VideoQueryArgs:QueryArgs,IVideoQueryArgs
    {
        public virtual string? Name { get; set; }
        public virtual DateTime? CreationTimeMin { get; set; }
        public virtual DateTime? CreationTimeMax { get; set; }
        public virtual TimeSpan? DurationMin { get; set; }
        public virtual TimeSpan? DurationMax { get; set; }
        public Guid? CreatorId { get; set; }
    }
}