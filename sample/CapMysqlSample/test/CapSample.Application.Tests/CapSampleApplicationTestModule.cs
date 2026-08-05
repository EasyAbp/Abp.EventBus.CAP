using Volo.Abp.Modularity;

namespace CapSample
{
    [DependsOn(
        typeof(CapSampleApplicationModule),
        typeof(CapSampleDomainTestModule)
        )]
    public class CapSampleApplicationTestModule : AbpModule
    {

    }
}