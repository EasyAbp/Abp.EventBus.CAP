using Volo.Abp.Dapper;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.EventBus.CAP.SqlServer
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpDapperModule)
    )]
    public class AbpEventBusCapSqlServerModule: AbpModule
    {
        
    }
}