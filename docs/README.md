# Abp.EventBus.CAP

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2FEasyAbp%2FAbp.EventBus.CAP%2Fmaster%2FDirectory.Build.props)](https://abp.io)
[![CAP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=cap&query=%2F%2FProject%2FPropertyGroup%2FCapPackageVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2FEasyAbp%2FAbp.EventBus.CAP%2Fmaster%2FDirectory.Build.props)](https://cap.dotnetcore.xyz)
[![NuGet](https://img.shields.io/nuget/v/EasyAbp.Abp.EventBus.CAP.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.Abp.EventBus.CAP)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.Abp.EventBus.CAP.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.Abp.EventBus.CAP)
[![Discord online](https://badgen.net/discord/online-members/S6QaezrCRq?label=Discord)](https://discord.gg/S6QaezrCRq)
[![GitHub stars](https://img.shields.io/github/stars/EasyAbp/Abp.EventBus.CAP?style=social)](https://www.github.com/EasyAbp/Abp.EventBus.CAP)

ABP vNext framework CAP EventBus module that integrated the [CAP](https://github.com/dotnetcore/CAP/) with the [ABP](https://github.com/abpframework/abp) framework.

## Installation

1. Install the following NuGet packages. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/docs/How-To.md#add-nuget-packages))

    * EasyAbp.Abp.EventBus.CAP

1. Add `DependsOn(typeof(AbpEventBusCapXxxModule))` attribute to configure the module dependencies. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/docs/How-To.md#add-module-dependencies))

1. Configure the CAP.
	```csharp
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		var configuration = context.Services.GetConfiguration();
		context.AddCapEventBus(capOptions =>
		{
			// capOptions.UseInMemoryStorage();
			capOptions.UseRabbitMQ("localhost");	// Configure the host of RabbitMQ
			capOptions.UseDashboard();	// CAP provides dashboard pages after the version 2.x
		});
	}
	```

## Usage

See the [ABP distributed event bus document](https://docs.abp.io/en/abp/latest/Distributed-Event-Bus) and the [CAP document](https://cap.dotnetcore.xyz/user-guide/en/getting-started/quick-start).