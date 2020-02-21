using Volo.Abp.Dapper;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace Yhd.Abp.CAP.SqlServer
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpDapperModule)
    )]
    public class CapSqlServerModule: AbpModule
    {
        
    }
}