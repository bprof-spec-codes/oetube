using OeTube.Domain.Entities;
using OeTube.Domain.Entities.Groups;

namespace OeTube.Data.QueryExtensions
{
    public class Membership
    {
        public OeTubeUser? User { get; set; }
        public Group? Group { get; set; }
    }
    public static class MembersQueryExtension
    {
        public static IQueryable<Group> GetJoinedGroups(this OeTubeDbContext context, Guid? requesterId)
        {
            if(requesterId is null)
            {
                return Enumerable.Empty<Group>().AsQueryable();
            }

            var groups = from membership in context.GetMemberships()
                          where membership.User!.Id==requesterId
                          select membership.Group;
            return groups;
        }
        public static IQueryable<OeTubeUser> GetExplicitMembers(this OeTubeDbContext context, Group group)
        {
            return from member in context.Set<Member>()
                   where member.GroupId == @group.Id
                   join user in context.OeTubeUsers
                   on member.UserId equals user.Id
                   select user;
        }
        public static IQueryable<OeTubeUser> GetMembers(this OeTubeDbContext context,Group group)
        {
            return from membership in context.GetMemberships()
                   where membership.Group!.Id == @group.Id
                   select membership.User;
        }
        public static IQueryable<Membership> GetMemberships(this OeTubeDbContext context)
        {
            var creators=from @group in context.Set<Group>()
                   where @group.CreatorId != null
                   join user in context.Set<OeTubeUser>()
                   on @group.CreatorId equals user.Id
                   select new Membership
                   {
                       Group = @group,
                       User = user
                   };
            var members= from member in context.Set<Member>()
                         join user in context.Set<OeTubeUser>()
                         on member.UserId equals user.Id
                         join @group in context.Set<Group>()
                         on member.GroupId equals @group.Id
                         select new Membership
                         {
                             Group = @group,
                             User = user
                         };
            var domainMembers= from emailDomain in context.Set<EmailDomain>()
                    join user in context.Set<OeTubeUser>()
                    on emailDomain.Domain equals user.EmailDomain
                    join @group in context.Set<Group>()
                    on emailDomain.GroupId equals @group.Id
                    select new Membership()
                    {
                        Group = @group,
                        User = user
                    };

            return creators.Union(members).Union(domainMembers).Distinct();
        }
    }
}