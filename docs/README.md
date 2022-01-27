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
    * EasyAbp.Abp.EventBus.Dashboard (if you need the CAP dashboard)
    * EasyAbp.Abp.EventBus.EntityFramework (if you need CAP transactional outbox)
    * EasyAbp.Abp.EventBus.MongoDB (coming soon...)
    * DotNetCore.CAP.SqlServer (or other DB providers if you are using EF Core)
    * DotNetCore.CAP.MongoDB (if you are using MongoDB)
    * DotNetCore.CAP.RabbitMQ (or other MQ providers)

1. Add `DependsOn(typeof(AbpEventBusCapXxxModule))` attribute to configure the module dependencies. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/docs/How-To.md#add-module-dependencies))

1. Configure the CAP.
    ```csharp
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        context.AddCapEventBus(capOptions =>
        {
            // If you are using EF, you need to add:
            options.SetCapDbConnectionString(configuration["ConnectionStrings:Default"]);
            options.UseEntityFramework<MyDbContext>();

            // CAP has multiple MQ implementations, e.g. RabbitMQ:
            options.UseRabbitMQ("localhost");
            
            // We provide permission named "CapDashboard.Manage" for authorization.
            options.UseAbpDashboard();
        });
    }
    ```

## Usage

See the [ABP distributed event bus document](https://docs.abp.io/en/abp/latest/Distributed-Event-Bus).

## How Do We Integrate CAP?

After ABP 5.0 released, the distributed event bus was redesigned. See: https://github.com/abpframework/abp/issues/6126

```c#
// ABP 5.0
Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true, bool useOutbox = true);

// ABP 4.0
Task PublishAsync<TEvent>(TEvent eventData);
```

Before ABP 5.0, when you invoke PublishAsync, the bus will push the event to MQ at once.

As you can see, after ABP 5.0, events are sent using outbox on UOW complete by default. CAP has a built-in transactional outbox, so we can implement it easily.

If you install the `EasyAbp.Abp.EventBus.EntityFramework` module, events are published with CAP's transactional outbox. Otherwise, they are published on UOW completed. See the [CapDistributedEventBus](https://github.com/EasyAbp/Abp.EventBus.CAP/blob/master/src/EasyAbp.Abp.EventBus.CAP/CapDistributedEventBus.cs) for more information.

But there are also some [problems](https://github.com/abpframework/abp/issues/6126#issuecomment-841888235) with CAP in ABP. In short, an ABP app could have more than one DB connection string, but CAP can only have one, this cannot be solved at present. So if there is not any DB connection string of the current UOW is equal to CAP's, events will be published after the UOW is completed.
