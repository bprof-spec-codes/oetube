using System.Threading.Tasks;

namespace OeTube.Data;

public interface IOeTubeDbSchemaMigrator
{
    Task MigrateAsync();
}
