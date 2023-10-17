using JetBrains.Annotations;
using OeTube.Entities;
using System.Runtime.Intrinsics.Arm;
using Volo.Abp.Domain.Entities;
using Volo.Abp;

namespace OeTube.Domain.Entities.Groups
{
    public class EmailDomain : Entity,IHasAtomicKey<string>
    {
        public Guid GroupId { get; private set; }
        public string Domain { get; private set; }

        string IHasAtomicKey<string>.AtomicKey => Domain;

        public EmailDomain(Guid groupId, [NotNull]string domain)
        {
            GroupId = groupId;
            Check.Length(domain, nameof(domain),
                        EmailDomainConstants.DomainMaxLength,
                        EmailDomainConstants.DomainMinLength);
            Domain = domain;
        }

        public override object[] GetKeys()
        {
            return new object[] { GroupId, Domain };
        }
    }
    public static class EmailDomainConstants
    {
        public const int DomainMaxLength = 255;
        public const int DomainMinLength = 3;
    }
}
