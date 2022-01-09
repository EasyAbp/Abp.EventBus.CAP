// Copyright (c) Easyabp Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace EasyAbp.Abp.EventBus.Cap
{
    [DependsOn(
        typeof(AbpEventBusModule),
        typeof(AbpUnitOfWorkModule)
    )]
    public class AbpEventBusCapModule : AbpModule
    {
    }
}
