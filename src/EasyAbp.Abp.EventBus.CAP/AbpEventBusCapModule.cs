// Copyright (c) Easyabp Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.EventBus.Cap
{
    [DependsOn(typeof(AbpEventBusModule))]
    public class AbpEventBusCapModule : AbpModule
    {
    }
}
