using EasyAbp.Abp.EventBus.CAP.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.Abp.EventBus.CAP.Permissions
{
    public class CapDashboardPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(CapDashboardPermissions.GroupName, L("Permission:CapDashboard"));
            myGroup.AddPermission(CapDashboardPermissions.Manage, L("Permission:Manage"), MultiTenancySides.Host);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CapDashboardResource>(name);
        }
    }
}
