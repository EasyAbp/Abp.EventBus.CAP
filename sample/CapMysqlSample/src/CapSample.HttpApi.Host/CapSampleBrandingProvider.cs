using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace CapSample
{
    [Dependency(ReplaceServices = true)]
    public class CapSampleBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "CapSample";
    }
}
