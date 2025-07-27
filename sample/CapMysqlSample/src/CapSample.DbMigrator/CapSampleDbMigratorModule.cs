using CapSample.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace CapSample.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(CapSampleEntityFrameworkCoreModule),
        typeof(CapSampleApplicationContractsModule)
        )]
    public class CapSampleDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
