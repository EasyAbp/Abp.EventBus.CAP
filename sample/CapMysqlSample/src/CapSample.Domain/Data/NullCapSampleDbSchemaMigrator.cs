using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace CapSample.Data
{
    /* This is used if database provider does't define
     * ICapSampleDbSchemaMigrator implementation.
     */
    public class NullCapSampleDbSchemaMigrator : ICapSampleDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}