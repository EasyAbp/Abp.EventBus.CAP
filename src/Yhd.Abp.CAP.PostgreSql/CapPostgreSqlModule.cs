using System;
using Volo.Abp.Dapper;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace Yhd.Abp.CAP.PostgreSql
{
    [DependsOn(
       typeof(AbpEntityFrameworkCorePostgreSqlModule),
       typeof(AbpDapperModule)
       )]
    public class CapPostgreSqlModule : AbpModule
    {

    }
}
