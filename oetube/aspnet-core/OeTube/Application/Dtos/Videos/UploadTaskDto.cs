namespace OeTube.Application.Dtos.Videos
{
    public class UploadTaskDto
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Arguments { get; set; } = string.Empty;
    }
}