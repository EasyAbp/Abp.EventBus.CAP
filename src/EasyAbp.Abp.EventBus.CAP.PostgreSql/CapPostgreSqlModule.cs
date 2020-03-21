using System;
using Volo.Abp.Dapper;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.EventBus.CAP.PostgreSql
{
    [DependsOn(
       typeof(AbpEntityFrameworkCorePostgreSqlModule),
       typeof(AbpDapperModule)
       )]
    public class CapPostgreSqlModule : AbpModule
    {

    }
}
