using System;
using System.Collections.Generic;
using System.Text;
using CapSample.Localization;
using Volo.Abp.Application.Services;

namespace CapSample
{
    /* Inherit your application services from this class.
     */
    public abstract class CapSampleAppService : ApplicationService
    {
        protected CapSampleAppService()
        {
            LocalizationResource = typeof(CapSampleResource);
        }
    }
}
