using CapSample.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace CapSample.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class CapSampleController : AbpControllerBase
    {
        protected CapSampleController()
        {
            LocalizationResource = typeof(CapSampleResource);
        }
    }
}