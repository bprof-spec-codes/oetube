using OeTube.Domain.Entities.Groups;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;

namespace OeTube.Domain.Entities
{
    public class OeTubeUser : AggregateRoot<Guid>, IHasCreationTime, IHasAtomicKey<Guid>, IHasName
    {
        public string Name { get; private set; }
        public string? AboutMe { get; private set; }
        public string EmailDomain { get; private set; }
        public DateTime CreationTime { get; private set; }

        Guid IHasAtomicKey<Guid>.AtomicKey => Id;

        private OeTubeUser()
        {
            Name = string.Empty;
            EmailDomain = string.Empty;
        }

        public OeTubeUser(IdentityUser user) : this()
        {
            Id = user.Id;
            CreationTime = DateTime.Now;
            SetName(user.UserName);
            string emailDomain = user.Email.Split("@")[1];
            Check.Length(emailDomain, nameof(emailDomain),
                         EmailDomainConstants.DomainMaxLength,
                         EmailDomainConstants.DomainMinLength);
            EmailDomain = emailDomain;
        }

        public OeTubeUser SetName([NotNull] string name)
        {
            Check.Length(name,
                         nameof(name),
                         OeTubeUserConstants.NameMaxLength,
                         OeTubeUserConstants.NameMinLength);
            Name = name;
            return this;
        }

        public OeTubeUser SetAboutMe(string? aboutMe)
        {
            Check.Length(aboutMe, nameof(aboutMe), OeTubeUserConstants.AboutMeMaxLength);
            AboutMe = aboutMe;
            return this;
        }
    }

    public static class OeTubeUserConstants
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;

        public const int AboutMeMaxLength = 1000;
    }
}