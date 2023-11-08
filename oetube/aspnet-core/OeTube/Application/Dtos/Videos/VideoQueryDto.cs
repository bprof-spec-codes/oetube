﻿using OeTube.Domain.Repositories.QueryArgs;

namespace OeTube.Application.Dtos.Videos
{

    public class VideoQueryDto : IVideoQueryArgs
    {
        public string? Name { get; set; }
        public DateTime? CreationTimeMin { get; set; }
        public DateTime? CreationTimeMax { get; set; }
        public TimeSpan? DurationMin { get; set; }
        public TimeSpan? DurationMax { get; set; }
        public int? ItemPerPage { get; set; }
        public int? Page { get; set; }
        public string? Sorting { get; set; }
    }
}