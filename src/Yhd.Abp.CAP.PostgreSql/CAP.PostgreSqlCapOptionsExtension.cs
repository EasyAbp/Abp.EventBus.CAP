// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using DotNetCore.CAP.Monitoring;
using DotNetCore.CAP.Persistence;
using DotNetCore.CAP.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace DotNetCore.CAP
{
    internal class PostgreSqlCapOptionsExtension<TDbContext> : ICapOptionsExtension
        where TDbContext : IEfCoreDbContext
    {
        private readonly Action<EFOptions> _configure;

        public PostgreSqlCapOptionsExtension(Action<EFOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<CapStorageMarkerService>();
            services.AddSingleton<IDataStorage, PostgreSqlDataStorage<TDbContext>>();
          
            services.AddSingleton<IStorageInitializer, PostgreSqlStorageInitializer<TDbContext>>();
            services.AddTransient<ICapTransaction, PostgreSqlCapTransaction>();
            services.AddTransient<IMonitoringApi, PostgreSqlMonitoringApi<TDbContext>>();

            services.Configure(_configure);
        }
    }
}