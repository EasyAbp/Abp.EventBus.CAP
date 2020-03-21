using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.EventBus.Cap
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 增加Cap分布式事件总线
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ServiceConfigurationContext AddCapEventBus(this ServiceConfigurationContext context, Action<CapOptions> capAction)
        {
            context.Services.AddCap(capAction);
            // 替换cap默认的消费者服务查找器
            context.Services.AddSingleton<IConsumerServiceSelector, ConsumerServiceSelector>();
            return context;
        }
    }
}
