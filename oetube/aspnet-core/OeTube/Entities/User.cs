using Volo.Abp.Domain.Entities;

namespace OeTube.Entities
{
    public class User : AggregateRoot<Guid>
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string AboutMe { get; private set; }
        public DateTime CreationTime { get; private set; }

        public User()
        {
            
        }

        public User(Guid id, string email, string name, DateTime creationTime)
        {
            Id = id;
            Email = email;
            Name = name;
            CreationTime = creationTime;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetAboutMe(string aboutMe)
        {
            AboutMe = aboutMe;
        }
    }
}
