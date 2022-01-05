// Copyright (c) Easyabp Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using DotNetCore.CAP.Serialization;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.EventBus.Cap
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceConfigurationContext AddCapEventBus(this ServiceConfigurationContext context, Action<CapOptions> capAction)
        {
            context.Services.AddSingleton<IConsumerServiceSelector, ConsumerServiceSelector>();
            context.Services.AddSingleton<IDistributedEventBus, CapDistributedEventBus>();
            context.Services.AddSingleton<ISerializer, AbpJsonSerializer>();
            
            context.Services.AddCap(capAction);

            return context;
        }
    }
}
