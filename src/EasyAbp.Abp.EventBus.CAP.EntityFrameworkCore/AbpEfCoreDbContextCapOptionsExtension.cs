using System;
using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;

namespace EasyAbp.Abp.EventBus.CAP;

public class AbpEfCoreDbContextCapOptionsExtension : ICapOptionsExtension
{
    public Type AbpEfDbContextType { get; init; }
    
    public void AddServices(IServiceCollection services)
    {
        services.Configure<AbpEfCoreDbContextCapOptions>(options =>
        {
            options.AbpEfDbContextType = AbpEfDbContextType;
        });
    }
}