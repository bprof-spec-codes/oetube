using System.Text.RegularExpressions;
using JetBrains.Annotations;
using OeTube.Entities;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;


namespace OeTube.Domain.Entities.Groups
{
  
    public class Group : AggregateRoot<Guid>, IHasCreationTime, IMayHaveCreator
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreationTime { get; private set; }
        public Guid? CreatorId { get; private set; }

        private readonly EntitySet<EmailDomain, string> emailDomains;
        private readonly EntitySet<Member, Guid> members;
        public virtual IReadOnlyEntitySet<Member, Guid> Members => members;
        public virtual IReadOnlyEntitySet<EmailDomain, string> EmailDomains => emailDomains;

        private Group()
        {
            members = new EntitySet<Member,Guid>();
            emailDomains = new EntitySet<EmailDomain,string>();
        }

        public Group(Guid id,[NotNull]string name, Guid creatorId)
        {
            Id = id;
            CreationTime = DateTime.Now;
            CreatorId = creatorId;
            SetName(name);
            members = new EntitySet<Member,Guid>();
            emailDomains = new EntitySet<EmailDomain,string>();
        }

        public Group SetDescription(string? description)
        {
            Check.Length(description, nameof(description), GroupConstants.DescriptionMaxLength);
            Description = description;
            return this;
        }
        public Group SetName([NotNull]string name)
        {
            Check.Length(name, nameof(name),
                         GroupConstants.NameMaxLength, 
                         GroupConstants.NameMinLength);
            Name = name;
            return this;
        }
        public Group AddEmailDomain([NotNull]string emailDomain)
        {
            if (!emailDomains.Add(new EmailDomain(Id,emailDomain)))
            {
                throw new ArgumentException();
            }
            return this;
        }
        public Group RemoveEmailDomain(string emailDomain)
        {
            if (!emailDomains.Remove(new EmailDomain(Id, emailDomain)))
            {
                throw new ArgumentException();
            }
            return this;
        }
        public Group AddMember(Guid userId)
        {
            if (CreatorId == userId)
            {
                throw new ArgumentException();
            }
            if (!members.Add(new Member(Id, userId))) 
            {
                throw new ArgumentException();   
            }
            return this;
        }

        public Group RemoveMember(Guid userId)
        {
            if (!members.Remove(new Member(Id, userId)))
            {
                throw new ArgumentException();
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
