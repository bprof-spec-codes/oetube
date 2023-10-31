namespace OeTube.Domain.Repositories.QueryArgs
{
    public interface IVideoQueryArgs : IQueryArgs
    {
        public string? Name { get; set; }
        public DateTime? CreationTimeMin { get; set; }
        public DateTime? CreationTimeMax { get; set; }
        public TimeSpan? DurationMin { get; set; }
        public TimeSpan? DurationMax { get; set; }
    }
}