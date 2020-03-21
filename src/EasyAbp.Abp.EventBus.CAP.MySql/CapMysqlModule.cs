using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Dapper;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.EventBus.CAP.MySql
{
    [DependsOn(
       typeof(AbpEntityFrameworkCoreMySQLModule),
       typeof(AbpDapperModule)
       )]
    public class CapMysqlModule : AbpModule
    {

    }
}
