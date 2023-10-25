using Microsoft.EntityFrameworkCore;
using OeTube.Data.Configurations;
using OeTube.Data.Configurations.Groups;
using OeTube.Data.Configurations.Playlists;
using OeTube.Data.Configurations.Videos;
using OeTube.Domain.Entities.Groups;
using OeTube.Domain.Entities.Playlists;
using OeTube.Domain.Entities.Videos;
using OeTube.Entities;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace OeTube.Data;

public class OeTubeDbContext : AbpDbContext<OeTubeDbContext>,ITenantManagementDbContext,IIdentityDbContext
{
    public DbSet<OeTubeUser> OeTubeUsers { get;private set; }
    public DbSet<Group> Groups { get;private set; }
    public DbSet<Playlist> Playlists { get; private set; }
    public DbSet<Video> Videos { get;private set; }

    public DbSet<Tenant> Tenants { get; private set; }

    public DbSet<TenantConnectionString> TenantConnectionStrings { get; private set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; private set; }

    public DbSet<OrganizationUnit> OrganizationUnits { get; private set; }

    public DbSet<IdentitySecurityLog> SecurityLogs { get; private set; }

    public DbSet<IdentityLinkUser> LinkUsers { get; private set; }

    public DbSet<IdentityUserDelegation> UserDelegations { get; private set; }

    public DbSet<IdentityUser> Users { get; private set; }

    public DbSet<IdentityRole> Roles { get; private set; }

    public OeTubeDbContext(DbContextOptions<OeTubeDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own entities here */

        builder.ApplyConfiguration(new OeTubeUserConfiguration());
        builder.ApplyConfiguration(new GroupConfiguration());
        builder.ApplyConfiguration(new MemberConfiguration());
        builder.ApplyConfiguration(new EmailDomainConfiguration());
        builder.ApplyConfiguration(new VideoConfiguration());
        builder.ApplyConfiguration(new AccessGroupConfiguration());
        builder.ApplyConfiguration(new PlaylistConfiguration());
        builder.ApplyConfiguration(new VideoItemConfiguration());
        builder.ApplyConfiguration(new VideoResolutionConfiguration());

    }

}
