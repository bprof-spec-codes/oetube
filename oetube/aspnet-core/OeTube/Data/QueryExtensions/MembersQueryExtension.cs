using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;

namespace OeTube.Data.QueryExtensions
{
    public static class MembersQueryExtension
    {
        public static IQueryable<Group> GetJoinedGroups(this OeTubeDbContext context, OeTubeUser user)
        {
            var result = from @group in context.Set<Group>()
                         where @group.CreatorId != user.Id
                         join member in context.GetMembers()
                         on @group.Id equals member.GroupId
                         where member.UserId == user.Id
                         select @group;
            return result;
        }

        public static IQueryable<OeTubeUser> GetMembers(this OeTubeDbContext context, Group group)
        {
            var explicitMembers = from member in context.Set<Member>()
                                  where member.GroupId == @group.Id
                                  join user in context.Set<OeTubeUser>()
                                  on member.UserId equals user.Id
                                  select user;

            var domainMembers = from emailDomain in context.Set<EmailDomain>()
                                where emailDomain.GroupId == @group.Id
                                join user in context.Set<OeTubeUser>()
                                on emailDomain.Domain equals user.EmailDomain
                                where user.Id != @group.CreatorId
                                select user;

            return explicitMembers.Concat(domainMembers).Distinct();
        }

        public static IQueryable<Member> GetMembers(this OeTubeDbContext context)
        {
            var domainMembers = from emailDomain in context.Set<EmailDomain>()
                                join user in context.Set<OeTubeUser>()
                                on emailDomain.Domain equals user.EmailDomain
                                select new Member(emailDomain.GroupId, user.Id);

            return context.Set<Member>().Concat(domainMembers).Distinct();
        }
    }
}