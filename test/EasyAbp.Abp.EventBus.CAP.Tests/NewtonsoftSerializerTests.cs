using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;

namespace EasyAbp.Abp.EventBus.CAP;

public class NewtonsoftSerializerTests : SerializerTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.PreConfigure<AbpJsonOptions>(options =>
        {
            options.UseHybridSerializer = false;
        });
    }
}