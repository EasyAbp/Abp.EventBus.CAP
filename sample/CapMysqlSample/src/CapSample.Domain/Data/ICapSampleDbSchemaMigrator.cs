using System.Threading.Tasks;

namespace CapSample.Data
{
    public interface ICapSampleDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
