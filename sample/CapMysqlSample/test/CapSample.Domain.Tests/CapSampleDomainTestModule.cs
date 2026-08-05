using CapSample.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace CapSample
{
    [DependsOn(
        typeof(CapSampleEntityFrameworkCoreTestModule)
        )]
    public class CapSampleDomainTestModule : AbpModule
    {

    }
}