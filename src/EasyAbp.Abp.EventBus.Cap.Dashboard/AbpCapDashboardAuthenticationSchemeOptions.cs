using Microsoft.AspNetCore.Authentication;

namespace EasyAbp.Abp.EventBus.CAP;

public class AbpCapDashboardAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public string PermissionName { get; set; }
}