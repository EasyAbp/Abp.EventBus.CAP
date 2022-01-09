// Copyright (c) Easyabp Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using System;
using DotNetCore.CAP.Serialization;
using EasyAbp.Abp.EventBus.Cap;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceConfigurationContext AddCapEventBus(this ServiceConfigurationContext context, Action<CapOptions> capAction)
        {
            context.Services.AddSingleton<IConsumerServiceSelector, AbpConsumerServiceSelector>();
            context.Services.AddSingleton<IDistributedEventBus, CapDistributedEventBus>();
            context.Services.AddSingleton<ISerializer, AbpJsonSerializer>();
            context.Services.AddTransient<IUnitOfWork, CapUnitOfWork>();

            context.Services.AddCap(capAction);

            return context;
        }
    }
}
