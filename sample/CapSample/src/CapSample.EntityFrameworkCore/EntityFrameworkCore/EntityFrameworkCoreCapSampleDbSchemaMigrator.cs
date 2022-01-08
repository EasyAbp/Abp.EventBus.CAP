using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CapSample.Data;
using Volo.Abp.DependencyInjection;

namespace CapSample.EntityFrameworkCore
{
    public class EntityFrameworkCoreCapSampleDbSchemaMigrator
        : ICapSampleDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreCapSampleDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the CapSampleDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<CapSampleDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
