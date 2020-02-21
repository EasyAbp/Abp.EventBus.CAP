// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotNetCore.CAP.Monitoring;
using DotNetCore.CAP.Persistence;
using DotNetCore.CAP.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Volo.Abp.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace DotNetCore.CAP
{
    internal class SqlServerCapOptionsExtension<TDbContext>: ICapOptionsExtension 
        where TDbContext : IEfCoreDbContext
    {
        private readonly Action<EFOptions> _configure;

        public SqlServerCapOptionsExtension(Action<EFOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<CapStorageMarkerService>();
            services.AddSingleton<IDataStorage, SqlServerDataStorage<TDbContext>>();
            
            services.TryAddSingleton<IStorageInitializer, SqlServerStorageInitializer<TDbContext>>();
            services.AddTransient<ICapTransaction, SqlServerCapTransaction>();
            services.AddTransient<IMonitoringApi, SqlServerMonitoringApi<TDbContext>>();
            //Add EFOptions
            services.Configure(_configure);
        } 
    }
}