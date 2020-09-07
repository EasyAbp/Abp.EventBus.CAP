// Copyright (c) Easyabp Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Volo.Abp.Dapper;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.EventBus.CAP.MySql
{
    [DependsOn(
       typeof(AbpEntityFrameworkCoreMySQLModule),
       typeof(AbpDapperModule)
       )]
    public class AbpEventBusCapMysqlModule : AbpModule
    {

    }
}
