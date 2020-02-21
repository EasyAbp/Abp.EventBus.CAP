// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotNetCore.CAP.Monitoring;
using DotNetCore.CAP.MySql;
using DotNetCore.CAP.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Volo.Abp.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace DotNetCore.CAP
{
    internal class MySqlCapOptionsExtension<TDbContext>: ICapOptionsExtension 
        where TDbContext : IEfCoreDbContext
    {
        private readonly Action<EFOptions> _configure;

        public MySqlCapOptionsExtension(Action<EFOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<CapStorageMarkerService>();
            services.AddSingleton<IDataStorage, MySqlDataStorage<TDbContext>>();
            
            services.TryAddSingleton<IStorageInitializer, MySqlStorageInitializer<TDbContext>>();
            services.AddTransient<ICapTransaction, MySqlCapTransaction>();
            services.AddTransient<IMonitoringApi, MySqlMonitoringApi<TDbContext>>();
            //Add EFOptions
            services.Configure(_configure);
        } 
    }
}