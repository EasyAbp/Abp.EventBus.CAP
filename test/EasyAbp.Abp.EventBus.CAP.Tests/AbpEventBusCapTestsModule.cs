using EasyAbp.Abp.EventBus.Cap;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.EventBus.CAP
{
    [DependsOn(
        typeof(AbpDataModule),
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(AbpEventBusCapModule)
        )]
    public class AbpEventBusCapTestsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.AddCapEventBus(options =>
            {

            });
        }
    }
}
