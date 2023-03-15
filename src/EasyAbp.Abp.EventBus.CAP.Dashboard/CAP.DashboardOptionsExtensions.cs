using System;
using DotNetCore.CAP;
using EasyAbp.Abp.EventBus.CAP;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class CapOptionsExtensions
    {
        public static CapOptions UseAbpDashboard(this CapOptions capOptions)
        {
            return capOptions.UseDashboard(options =>
            {
                options.UseAuth = true;
                options.DefaultAuthenticationScheme = AbpCapDashboardAuthenticationHandler.SchemeName;
            });
        }
        
        public static CapOptions UseAbpDashboard(this CapOptions capOptions, Action<DashboardOptions> options)
        {
            options += dashboardOptions =>
            {
                dashboardOptions.DefaultAuthenticationScheme ??= AbpCapDashboardAuthenticationHandler.SchemeName;
            };
            
            return capOptions.UseDashboard(options);
        }
    }
}