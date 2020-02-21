using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace App2
{
    [ConnectionStringName("Default")]
    public interface IAppDbContext : IEfCoreDbContext
    {

    }
}
