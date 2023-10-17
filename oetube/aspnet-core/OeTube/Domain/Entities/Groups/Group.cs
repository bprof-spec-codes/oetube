using System.Text.RegularExpressions;
using JetBrains.Annotations;
using OeTube.Entities;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;


namespace OeTube.Domain.Entities.Groups
{
  
    public class Group : AggregateRoot<Guid>, IHasCreationTime, IMayHaveCreator, IHasAtomicKey<Guid>
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreationTime { get; private set; }
        public Guid? CreatorId { get; private set; }

        private readonly EntitySet<EmailDomain, string> emailDomains;
        private readonly EntitySet<Member, Guid> members;
        public virtual IReadOnlyEntitySet<Member, Guid> Members => members;
        public virtual IReadOnlyEntitySet<EmailDomain, string> EmailDomains => emailDomains;

        Guid IHasAtomicKey<Guid>.AtomicKey => Id;

        private Group()
        {
            Name = string.Empty;
            members = new EntitySet<Member,Guid>();
            emailDomains = new EntitySet<EmailDomain,string>();
        }

        public Group(Guid id,string name, Guid creatorId):this()
        {
            Id = id;
            CreationTime = DateTime.Now;
            CreatorId = creatorId;
            SetName(name);
        }

        public Group SetDescription(string? description)
        {
            Check.Length(description, nameof(description), GroupConstants.DescriptionMaxLength);
            Description = description;
            return this;
        }
        public Group SetName(string name)
        {
            Check.Length(name, nameof(name),
                         GroupConstants.NameMaxLength, 
                         GroupConstants.NameMinLength);
            Name = name;
            return this;
        }

        public Group UpdateEmailDomains(IEnumerable<string> emailDomains)
        {
            this.emailDomains.Clear();
            foreach (var item in emailDomains)
            {
                this.emailDomains.Add(new EmailDomain(Id, item));
            }
            return this;
        }
    

    }
    public static class GroupConstants
    {
        public const int NameMaxLength = 100;
        public const int NameMinLength = 2;

        public const int DescriptionMaxLength = 1000;
    }
}
