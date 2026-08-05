using CapSample.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace CapSample.Permissions
{
    public class CapSamplePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(CapSamplePermissions.GroupName);
            //Define your own permissions here. Example:
            //myGroup.AddPermission(CapSamplePermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CapSampleResource>(name);
        }
    }
}
