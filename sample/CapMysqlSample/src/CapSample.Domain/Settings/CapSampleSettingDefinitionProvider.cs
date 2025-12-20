using Volo.Abp.Settings;

namespace CapSample.Settings
{
    public class CapSampleSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(CapSampleSettings.MySetting1));
        }
    }
}
