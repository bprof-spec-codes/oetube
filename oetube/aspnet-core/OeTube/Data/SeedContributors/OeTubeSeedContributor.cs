using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OeTube.Data.Repositories.Playlists;
using OeTube.Data.Repositories.Repos.GroupRepos;
using OeTube.Data.Repositories.Videos;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Infrastructure.Videos;
using OeTube.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

using Abp = Volo.Abp.Identity;

namespace OeTube.Data.SeedContributors
{
    public class OeTubeSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ILookupNormalizer _lookUpNormalizer;
        private readonly IOptions<IdentityOptions> _options;
        private readonly GroupRepository _groupRepository;
        private readonly VideoRepository _videoRepository;
        private readonly PlaylistRepository _playlistRepository;
        private readonly IDefaultImageUploadHandler _defaultImageUploadHandler;
        public OeTubeSeedContributor(IGuidGenerator guidGenerator,
                                       IdentityUserManager userManager,
                                       IdentityRoleManager roleManager,
                                       IIdentityRoleRepository roleRepository,
                                       IIdentityUserRepository userRepository,
                                       ILookupNormalizer lookUpNormalizer,
                                       IOptions<IdentityOptions> options,
                                       GroupRepository groupRepository,
                                       VideoRepository videoRepository,
                                       PlaylistRepository playlistRepository,
                                       IDefaultImageUploadHandler defaultImageUploadHandler)
        {
            _guidGenerator = guidGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _lookUpNormalizer = lookUpNormalizer;
            _options = options;
            _groupRepository = groupRepository;
            _videoRepository = videoRepository;
            _playlistRepository = playlistRepository;
            _defaultImageUploadHandler = defaultImageUploadHandler;
        }

        [UnitOfWork]
        public void SeedEmailDomains(Group group, params string[] emailDomains)
        {
            group.UpdateEmailDomains(emailDomains);
        }

        [UnitOfWork]
        public async Task SeedMembersAsync(Group group, params Guid[] members)
        {
            await _groupRepository.UpdateMembersAsync(group, members);
        }

        [UnitOfWork]
        public async Task<Group> SeedGroupAsync(string name, Abp.IdentityUser user)
        {
            var group = await _groupRepository.FindAsync((g) => g.Name == name & g.CreatorId == user.Id);
            if (group == null)
            {
                group = new Group(_guidGenerator.Create(), name, user.Id);
                await _groupRepository.InsertAsync(group);
            }
            return group;
        }

        [UnitOfWork]
        public async Task<Abp.IdentityUser> SeedUserAsync(string userName, string email, string password, string? roleName = null)
        {
            await _options.SetAsync();

            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookUpNormalizer.NormalizeName(userName));
            if (user != null)
            {
                return user;
            }

            user = new Abp.IdentityUser(_guidGenerator.Create(), userName, email)
            {
                Name = userName
            };
            var userResult = await _userManager.CreateAsync(user, password, validatePassword: true);
            userResult.CheckErrors();
            if (roleName != null)
            {
                var role = await _roleRepository.FindByNormalizedNameAsync
                    (_lookUpNormalizer.NormalizeName(roleName));
                if (role == null)
                {
                    role = new Abp.IdentityRole(_guidGenerator.Create(), roleName)
                    {
                        IsPublic = true,
                        IsStatic = true
                    };
                    var roleResult = await _roleManager.CreateAsync(role);
                    roleResult.CheckErrors();
                }
                var userRole = await _userManager.AddToRoleAsync(user, roleName);
                userRole.CheckErrors();
            }

            return user;
        }

        [UnitOfWork]
        public async Task SetDefaultImageAsync<TRelatedType>(string filename)
        {
            var path = Path.Combine(nameof(Data), nameof(SeedContributors), "Files",filename);
            using var stream = File.OpenRead(path);
            var content = await ByteContent.FromStreamAsync(Path.GetExtension(path), stream);
            await _defaultImageUploadHandler.HandleFileAsync<TRelatedType>(content);

        }


        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            await SetDefaultImageAsync<Group>("group-default.png");
            await SetDefaultImageAsync<OeTubeUser>("user-default.png");
            string obudaMail = "@uni-obuda.hu";
            string studObudaMail = "@stud.uni-obuda.hu";
            string gmail = "@gmail.com";
            string admin = "Admin";

            string userName = "TestAdmin1";
            var admin1 = await SeedUserAsync(userName, userName + obudaMail, userName + "!", admin);
            userName = "TestAdmin2";
            var admin2 = await SeedUserAsync(userName, userName + studObudaMail, userName + "!", admin);

            userName = "TestUser1";
            var user1 = await SeedUserAsync(userName, userName + studObudaMail, userName + "!");
            userName = "TestUser2";
            var user2 = await SeedUserAsync(userName, userName + obudaMail, userName + "!");
            userName = "TestUser3";
            var user3 = await SeedUserAsync(userName, userName + studObudaMail, userName + "!");
            userName = "TestUser4";
            var user4 = await SeedUserAsync(userName, userName + gmail, userName + "!");
            userName = "TestUser5";
            var user5 = await SeedUserAsync(userName, userName + gmail, userName + "!");

            var oe = await SeedGroupAsync("Oe", admin1);
            var oestud = await SeedGroupAsync("OeStud", admin2);
            var random = await SeedGroupAsync("Random", user2);
            var empty = await SeedGroupAsync("Empty", user1);

            await SeedMembersAsync(random, user4.Id, user5.Id, user3.Id);
            SeedEmailDomains(oe, "uni-obuda.hu", "stud.uni-obuda.hu");
            SeedEmailDomains(oestud, "stud.uni-obuda.hu");
            SeedEmailDomains(random, "stud.uni-obuda.hu");
        }
    }
}