using EasyAbp.Abp.EventBus.CAP;
using EasyAbp.Abp.EventBus.CAP.Localization;
using EasyAbp.Abp.EventBus.CAP.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.Abp.EventBus.Cap
{
    [DependsOn(typeof(AbpAuthorizationModule))]
    public class AbpEventBusCapDashboardModule : AbpModule
    {
        public static string CapDashboardAuthenticationPolicy { get; set; } = "CapDashboardAuthenticationPolicy";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services
                .AddAuthorization(options =>
                {
                    options.AddPolicy(CapDashboardAuthenticationPolicy, policy => policy
                        .AddAuthenticationSchemes(AbpCapDashboardAuthenticationHandler.SchemeName)
                        .RequireAuthenticatedUser());
                })
                .AddAuthentication()
                .AddScheme<AbpCapDashboardAuthenticationSchemeOptions, AbpCapDashboardAuthenticationHandler>(
                    AbpCapDashboardAuthenticationHandler.SchemeName,
                    options => { options.PermissionName = CapDashboardPermissions.Manage; });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpEventBusCapDashboardModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<CapDashboardResource>("en")
                    .AddVirtualJson("/Localization/CapDashboard");

                options.DefaultResourceType = typeof(CapDashboardResource);
            });
        }
    }
}