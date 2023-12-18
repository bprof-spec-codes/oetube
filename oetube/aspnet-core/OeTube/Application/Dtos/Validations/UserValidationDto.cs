using OeTube.Domain.Entities;

namespace OeTube.Application.Dtos.Validations
{
    public class UserValidationDto
    {
        public int NameMinLength => OeTubeUserConstants.NameMinLength;
        public int NameMaxLength => OeTubeUserConstants.NameMaxLength;
        public string NameMessage => ValidationsDto.GetDefaultStringLengthMessage("Name", NameMinLength, NameMaxLength);
        public int AboutMeMaxLength => OeTubeUserConstants.AboutMeMaxLength;
        public string AboutMeMessage => ValidationsDto.GetDefaultStringLengthMessage("About Me", null, AboutMeMaxLength);
    }
}
