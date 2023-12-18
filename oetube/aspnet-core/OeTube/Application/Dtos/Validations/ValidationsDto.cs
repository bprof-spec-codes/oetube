namespace OeTube.Application.Dtos.Validations
{
    public class ValidationsDto
    {
        public static string GetDefaultStringLengthMessage(string name, int? min=null, int? max=null)
        {
            if (min != null && max != null)
            {
                return $"{name} must be between {min} to {max} characters!";
            }
            else if (max == null)
            {
                return $"{name} must be at least {min} characters long!";
            }
            else if (min == null)
            {
                return $"{name} must be maximum {max} characters long!";
            }
            else
            {
                return "";
            }
        }

        public UserValidationDto User { get; } = new UserValidationDto();
        public VideoValidationDto Video { get; } = new VideoValidationDto();
        public PlaylistValidationDto Playlist { get; } = new PlaylistValidationDto();
        public GroupValidationDto Group { get; } = new GroupValidationDto();
    }
}
