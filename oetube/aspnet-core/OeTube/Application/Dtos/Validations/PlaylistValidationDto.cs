using OeTube.Domain.Entities.Playlists;

namespace OeTube.Application.Dtos.Validations
{
    public class PlaylistValidationDto
    {
        public int NameMinLength =>PlaylistConstants.NameMinLength;
        public int NameMaxLength =>PlaylistConstants.NameMaxLength;
        public string NameMessage => ValidationsDto.GetDefaultStringLengthMessage("Name", NameMinLength, NameMaxLength);
        public int DescriptionMaxLength =>PlaylistConstants.DescriptionMaxLength;
        public string DescriptionMessage => ValidationsDto.GetDefaultStringLengthMessage("Description", null, DescriptionMaxLength);
    }
}
