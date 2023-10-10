using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Repositories;
using OeTube.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace OeTube.Domain.Services
{
    public class GroupMemberManager : DomainService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IIdentityUserRepository _userRepository;

        public GroupMemberManager(IGroupRepository groupRepository, IIdentityUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public async Task<Group> UpdateMembersAsync(Group group, IEnumerable<Guid> members, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            members = members.Distinct();

            var users = await _userRepository.GetListByIdsAsync(members,false,cancellationToken);

            if(group.CreatorId!=null)
            {
                users.Remove(users.FirstOrDefault(u=>u.Id==group.CreatorId));
            }

            if (users.Count != members.Count())
            {
                throw new EntityNotFoundException(typeof(OeTubeUser));
            }

            await _groupRepository.UpdateMembersAsync(group, users, autoSave, cancellationToken);
            return group;
        }
    }
}
