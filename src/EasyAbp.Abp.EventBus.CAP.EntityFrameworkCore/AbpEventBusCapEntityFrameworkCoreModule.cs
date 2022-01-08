using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

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
