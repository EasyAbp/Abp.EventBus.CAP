// Copyright (c) Easyabp Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotNetCore.CAP;
using EasyAbp.Abp.EventBus.CAP;
using Volo.Abp.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class CapOptionsExtensions
    {
        public static CapOptions SetDbContextForCap<TAbpDbContext>(this CapOptions options)
            where TAbpDbContext : IAbpEfCoreDbContext
        {
            options.RegisterExtension(new AbpEfCoreDbContextCapOptionsExtension
            {
                AbpEfDbContextType = typeof(TAbpDbContext)
            });
            
            return options;
        }
    }
}
