using OeTube.Domain.Entities.Videos;

namespace OeTube.Application.Dtos.Validations
{
    public class VideoValidationDto
    {
        public int NameMinLength => VideoConstants.NameMinLength;
        public int NameMaxLength => VideoConstants.NameMaxLength;
        public string NameMessage => ValidationsDto.GetDefaultStringLengthMessage("Name", NameMinLength, NameMaxLength);
        public int DescriptionMaxLength => VideoConstants.DescriptionMaxLength;
        public string DescriptionMessage => ValidationsDto.GetDefaultStringLengthMessage("Description", null, DescriptionMaxLength);
    }
}
