using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.EventBus.Cap
{
    [DependsOn(typeof(AbpEventBusCapModule))]
    public class AbpEventBusCapEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<ICapTransactionApiFactory, EfCoreCapTransactionApiFactory>();
        }
    }
}
