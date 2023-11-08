﻿namespace OeTube.Domain.Repositories.QueryArgs
{
    public interface IGroupQueryArgs : IQueryArgs
    {
        string? Name { get; set; }
        DateTime? CreationTimeMin { get; set; }
        DateTime? CreationTimeMax { get; set; }
    }
    public class GroupQueryArgs:QueryArgs,IGroupQueryArgs
    {
        public string? Name { get; set; }
        public DateTime? CreationTimeMin { get; set; }
        public DateTime? CreationTimeMax { get; set; }
    }
    
}