using OeTube.Domain.Entities.Groups;

namespace OeTube.Application.Dtos.Validations
{
    public class GroupValidationDto
    {
        public int DomainMaxLength =>EmailDomainConstants.DomainMaxLength;
        public int DomainMinLength => EmailDomainConstants.DomainMinLength;
        public string DomainMessage => ValidationsDto.GetDefaultStringLengthMessage("Email domain", DomainMinLength, DomainMaxLength);
        public int NameMaxLength => GroupConstants.NameMaxLength;
        public int NameMinLength => GroupConstants.NameMinLength;
        public string NameMessage => ValidationsDto.GetDefaultStringLengthMessage("Name", NameMinLength, NameMaxLength);
        public int DescriptionMaxLength => GroupConstants.DescriptionMaxLength;
        public string DescriptionMessage => ValidationsDto.GetDefaultStringLengthMessage("Description", null, DescriptionMaxLength);
    }
}
