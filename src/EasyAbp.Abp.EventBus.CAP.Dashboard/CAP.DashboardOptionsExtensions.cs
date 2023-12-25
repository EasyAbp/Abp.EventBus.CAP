using System;
using DotNetCore.CAP;
using EasyAbp.Abp.EventBus.Cap;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class CapOptionsExtensions
    {
        public static CapOptions UseAbpDashboard(this CapOptions capOptions)
        {
            return capOptions.UseDashboard(options =>
            {
                options.AuthorizationPolicy = AbpEventBusCapDashboardModule.CapDashboardAuthenticationPolicy;
            });
        }

        public static CapOptions UseAbpDashboard(this CapOptions capOptions, Action<DashboardOptions> options)
        {
            options += dashboardOptions =>
            {
                dashboardOptions.AuthorizationPolicy =
                    AbpEventBusCapDashboardModule.CapDashboardAuthenticationPolicy;
            };

            return capOptions.UseDashboard(options);
        }
    }
}