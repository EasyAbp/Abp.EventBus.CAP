using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.EventBus.Cap
{

    [DependsOn(typeof(AbpEventBusModule))]
    public class CapModule:AbpModule
    {
        
    }
}
