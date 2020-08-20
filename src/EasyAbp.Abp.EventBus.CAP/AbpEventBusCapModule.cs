using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.EventBus.Cap
{
    [DependsOn(typeof(AbpEventBusModule))]
    public class AbpEventBusCapModule : AbpModule
    {
    }
}
