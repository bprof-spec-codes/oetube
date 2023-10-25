using OeTube.Infrastructure.FF;

namespace OeTube.Application.Dtos.Videos
{
    public class VideoUploadStateDto
    {
        public Guid Id { get; set; }
        public bool IsCompleted => RemainingTasks.Count == 0;
        public string OutputFormat { get; set; } = string.Empty;
        public List<UploadTask> RemainingTasks { get; set; } = new List<UploadTask>();
    }



}
