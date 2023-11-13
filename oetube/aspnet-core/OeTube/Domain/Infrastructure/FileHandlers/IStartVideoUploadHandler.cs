﻿using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Infrastructure.FileHandlers
{
    public class StartVideoUploadHandlerArgs
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AccessType Access { get; set; }
        public Guid? CreatorId { get; set; }
    }

    public interface IStartVideoUploadHandler : IContentFileHandler<StartVideoUploadHandlerArgs, Video>
    {


    }
}
