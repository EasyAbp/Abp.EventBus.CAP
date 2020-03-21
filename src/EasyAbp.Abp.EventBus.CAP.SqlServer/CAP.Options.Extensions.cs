// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotNetCore.CAP;
using System;
using Volo.Abp.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class CapOptionsExtensions
    {

        public static CapOptions UseEntityFramework<TContext>(this CapOptions options)
            where TContext : IEfCoreDbContext
        {
            return options.UseEntityFramework<TContext>(opt => { });
        }

        public static CapOptions UseEntityFramework<TContext>(this CapOptions options, Action<EFOptions> configure)
            where TContext : IEfCoreDbContext
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new SqlServerCapOptionsExtension<TContext>(x =>
            {
                configure(x);
                x.DbContextType = typeof(TContext);
                x.Version = options.Version;
            }));

            return options;
        }
    }
}