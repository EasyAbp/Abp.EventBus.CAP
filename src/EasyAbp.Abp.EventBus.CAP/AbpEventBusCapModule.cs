// Copyright (c) Easyabp Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EasyAbp.Abp.EventBus.CAP;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;
using Volo.Abp.Uow;

namespace EasyAbp.Abp.EventBus.Cap
{
    [DependsOn(
        typeof(AbpEventBusModule),
        typeof(AbpUnitOfWorkModule)
    )]
    public class AbpEventBusCapModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AddEventHandlers(context.Services);
        }
        private static void AddEventHandlers(IServiceCollection services)
        {
            var distributedHandlers = new List<Type>();

            services.OnRegistred(context =>
            {
                if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(ICapDistributedEventHandler<>)))
                {
                    distributedHandlers.Add(context.ImplementationType);
                }
            });

            services.Configure<AbpDistributedEventBusOptions>(options =>
            {
                options.Handlers.AddIfNotContains(distributedHandlers);
            });
        }
    }
}
