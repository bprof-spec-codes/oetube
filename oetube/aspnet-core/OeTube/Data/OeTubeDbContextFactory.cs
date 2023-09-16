using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OeTube.Data;

public class OeTubeDbContextFactory : IDesignTimeDbContextFactory<OeTubeDbContext>
{
    public OeTubeDbContext CreateDbContext(string[] args)
    {

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<OeTubeDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new OeTubeDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
